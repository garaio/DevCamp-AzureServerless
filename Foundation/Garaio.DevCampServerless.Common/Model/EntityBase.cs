using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;

namespace Garaio.DevCampServerless.Common.Model
{
    public abstract class EntityBase : TableEntity
    {
        public const string GlobalPartitionKey = "GA";

        public EntityBase()
        {
            PartitionKey = GlobalPartitionKey;
            RowKey = NewRowKey;
        }
        public static string NewRowKey => Guid.NewGuid().ToString();

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
