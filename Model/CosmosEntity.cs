using System;
using Newtonsoft.Json;

namespace RocketAnt.Function
{
    public abstract class CosmosEntity
    {
        public CosmosEntity()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        [JsonIgnore]
        public abstract string PartitionKey { get; }

        public string Id { get; set; }

        public DateTime UpdatedAt { get; }
    }
}