using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;

namespace Garaio.DevCampServerless.Common.Model
{
    public abstract class EntityBase : TableEntity
    {
        public const string KeySeparator = "|";
        public const string DefaultPartitionKey = "devcamp";

        public EntityBase()
        {
            PartitionKey = DefaultPartitionKey;
            RowKey = NewRowKey;
        }

        public static string NewRowKey => Guid.NewGuid().ToString();

        public static (string partitionKey, string rowKey) ParseKeys(string entityKey)
        {
            var elements = entityKey?.Split(new[] { KeySeparator }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();

            if (elements.Length == 2)
                return (elements[0], elements[1]);

            return default;
        }

        public string EntityKey => PartitionKey + KeySeparator + RowKey;

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var results = base.WriteEntity(operationContext);
            EntityJsonPropertyConverter.Serialize(this, results);
            return results;
        }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);
            EntityJsonPropertyConverter.Deserialize(this, properties);
        }
    }
}
