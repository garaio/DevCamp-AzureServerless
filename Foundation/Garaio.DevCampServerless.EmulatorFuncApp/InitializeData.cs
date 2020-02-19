using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Garaio.DevCampServerless.Common.Model;

namespace Garaio.DevCampServerless.EmulatorFuncApp
{
    public static class InitializeData
    {
        [FunctionName(nameof(InitializeData))]
        public static async Task Run([TimerTrigger("0 0 0 1 1 *", RunOnStartup = true)]TimerInfo timer, ExecutionContext context, ILogger log)
        {
            log.LogInformation($"Data initializer triggered");

            string storageConnectionString = Environment.GetEnvironmentVariable(Constants.Configurations.StorageConnectionString);
            CloudTableClient tableClient = CloudStorageAccount.Parse(storageConnectionString).CreateCloudTableClient(new TableClientConfiguration());
            
            await CheckAndInitializeTable<Technology>(tableClient,context.FunctionAppDirectory, Constants.Data.TechnologiesFile, log);
            await CheckAndInitializeTable<TechnologyLink>(tableClient, context.FunctionAppDirectory, Constants.Data.TechnologyLinksFile, log);
        }

        private static async Task CheckAndInitializeTable<T>(CloudTableClient tableClient, string funcAppDirectory, string seedFilename, ILogger log) where T : EntityBase, new()
        {
            var tableName = typeof(T).Name.ToLower();
            CloudTable table = tableClient.GetTableReference(tableName);

            var exists = await table.ExistsAsync();
            if (exists)
                return;

            await table.CreateAsync();

            var rowNumber = 0;
            var batchOperation = new TableBatchOperation();

            var seedPath = Path.Combine(funcAppDirectory, Constants.Data.Directory, seedFilename);
            var seedJson = File.ReadAllText(seedPath);
            var seedEntries = JsonConvert.DeserializeObject<ICollection<T>>(seedJson);

            foreach (var entry in seedEntries)
            {
                batchOperation.Add(TableOperation.InsertOrReplace(entry));

                if (batchOperation.Count > 99)
                {
                    table.ExecuteBatch(batchOperation);
                    batchOperation.Clear();
                }

                rowNumber++;
            }

            if (batchOperation.Count > 0)
            {
                table.ExecuteBatch(batchOperation);
                batchOperation.Clear();
            }

            log.LogInformation($"{seedEntries.Count} {tableName}s initially seeded");
        }
    }
}
