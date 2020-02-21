using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Garaio.DevCampServerless.Common.Model;

namespace Garaio.DevCampServerless.ServiceFuncApp
{
    public class EntityManager<T> where T : EntityBase, new()
    {
        
        private readonly CloudTable _table;
        private readonly ILogger _log;

        public EntityManager(CloudTable table, ILogger log)
        {
            _table = table ?? throw new ArgumentNullException(nameof(table));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }
        
        public async Task<IList<T>> GetAllAsync()
        {
            try
            {
                return _table.CreateQuery<T>().Where(x => x.PartitionKey == EntityBase.GlobalPartitionKey).ToArray();
            }
            catch (StorageException e)
            {
                _log.LogError(e, $"Unable to access data in table '{_table.Name}'");
                throw;
            }
        }

        public async Task<IList<T>> GetWhereAsync(Func<T, bool> predicate)
        {
            try
            {
                return _table.CreateQuery<T>().Where(x => x.PartitionKey == EntityBase.GlobalPartitionKey).Where(predicate).ToArray();
            }
            catch (StorageException e)
            {
                _log.LogError(e, $"Unable to access data in table '{_table.Name}'");
                throw;
            }
        }

        public async Task<T> GetAsync(string key)
        {
            try
            {
                TableOperation retrieveOperation = TableOperation.Retrieve<T>(EntityBase.GlobalPartitionKey, key);
                TableResult result = await _table.ExecuteAsync(retrieveOperation);
                
                return result.Result as T;
            }
            catch (StorageException e)
            {
                _log.LogError(e, $"Unable to access data in table '{_table.Name}'");
                throw;
            }
        }

        public async Task<T> CreateOrUpdate(T entity)
        {
            try
            {
                var key = entity.RowKey;
                var exists = !string.IsNullOrEmpty(key) && _table.CreateQuery<T>().Where(x => x.PartitionKey == EntityBase.GlobalPartitionKey && x.RowKey == key).AsEnumerable().Any();

                TableOperation insertOrMergeOperation = TableOperation.InsertOrReplace(entity);
                TableResult result = await _table.ExecuteAsync(insertOrMergeOperation);

                var metric = string.Format(exists ? Constants.Metrics.EntityUpdatedPattern : Constants.Metrics.EntityCreatedPattern, typeof(T).Name);
                _log.LogMetric(metric, 1, new Dictionary<string, object> { { "key", key } });

                return result.Result as T;
            }
            catch (StorageException e)
            {
                _log.LogError(e, $"Unable to access data in table '{_table.Name}'");
                throw;
            }
        }

        /// <summary>
        /// Note: This method fails when change-set is bigger than 100 entries (maximal batch size)
        /// </summary>
        public async Task<int> Synchronize(Func<T, bool> predicate, ICollection<T> entities)
        {
            try
            {
                var existingKeys = _table.CreateQuery<T>().Where(x => x.PartitionKey == EntityBase.GlobalPartitionKey).Where(predicate).Select(x => x.RowKey).ToArray();

                var created = 0;
                var updated = 0;
                var deleted = 0;

                var batchOperation = new TableBatchOperation();

                foreach (var entity in entities.Where(e => !existingKeys.Contains(e.RowKey)))
                {
                    batchOperation.Add(TableOperation.InsertOrReplace(entity));
                    created++;
                }

                foreach (var entity in entities.Where(e => existingKeys.Contains(e.RowKey)))
                {
                    batchOperation.Add(TableOperation.InsertOrReplace(entity));
                    updated++;
                }

                foreach (var entityKey in existingKeys.Where(k => !entities.Any(e => e.RowKey == k)))
                {
                    var entity = await GetAsync(entityKey);

                    batchOperation.Add(TableOperation.Delete(entity));
                    deleted++;
                }

                TableBatchResult result = null;
                if (batchOperation.Count > 0)
                    result = await _table.ExecuteBatchAsync(batchOperation);

                if (created > 0)
                {
                    var metric = string.Format(Constants.Metrics.EntityCreatedPattern, typeof(T).Name);
                    _log.LogMetric(metric, created);
                }
                if (updated > 0)
                {
                    var metric = string.Format(Constants.Metrics.EntityUpdatedPattern, typeof(T).Name);
                    _log.LogMetric(metric, updated);
                }
                if (deleted > 0)
                {
                    var metric = string.Format(Constants.Metrics.EntityDeletedPattern, typeof(T).Name);
                    _log.LogMetric(metric, deleted);
                }

                return created + updated + deleted;
            }
            catch (StorageException e)
            {
                _log.LogError(e, $"Unable to access data in table '{_table.Name}'");
                throw;
            }
        }

        public async Task<bool> Delete(string key)
        {
            try
            {
                var entity = await GetAsync(key);
                if (entity == null)
                    return false;

                TableOperation deleteOperation = TableOperation.Delete(entity);
                TableResult result = await _table.ExecuteAsync(deleteOperation);

                var metric = string.Format(Constants.Metrics.EntityDeletedPattern, typeof(T).Name);
                _log.LogMetric(metric, 1, new Dictionary<string, object> { { "key", key } });

                return true;
            }
            catch (StorageException e)
            {
                _log.LogError(e, $"Unable to access data in table '{_table.Name}'");
                throw;
            }
        }

        public async Task<int> DeleteWhere(Func<T, bool> predicate)
        {
            try
            {
                var entities = await GetWhereAsync(predicate);
                var count = 0;

                foreach (var entity in entities)
                {
                    TableOperation deleteOperation = TableOperation.Delete(entity);
                    TableResult result = await _table.ExecuteAsync(deleteOperation);

                    count++;
                }

                var metric = string.Format(Constants.Metrics.EntityDeletedPattern, typeof(T).Name);
                _log.LogMetric(metric, count);

                return count;
            }
            catch (StorageException e)
            {
                _log.LogError(e, $"Unable to access data in table '{_table.Name}'");
                throw;
            }
        }
    }

    public static class EntityManager
    {
        private static CloudTableClient tableClient;

        public static EntityManager<T> Get<T>(ILogger log) where T : EntityBase, new()
        {
            if (tableClient == null)
            {
                string storageConnectionString = Environment.GetEnvironmentVariable(Constants.Configurations.StorageConnectionString);

                tableClient = CloudStorageAccount.Parse(storageConnectionString).CreateCloudTableClient(new TableClientConfiguration());
            }

            var tableName = typeof(T).Name.ToLower();
            CloudTable table = tableClient.GetTableReference(tableName);
            if (table.CreateIfNotExists())
            {
                log.LogInformation($"Created table '{tableName}'");
            }

            return new EntityManager<T>(table, log);
        }
    }
}
