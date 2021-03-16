using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace RocketAnt.Function
{
    public abstract class CosmosRepository<T> : ICosmosRepository<T> where T : CosmosEntity
    {
        protected readonly CosmosClient cosmosClient;
        protected readonly string containerName;

        protected Container GetContainer()
        {
            return this.cosmosClient.GetContainer("rocketant", containerName);
        }

        protected CosmosRepository(CosmosClient cosmosClient, string containerName)
        {
            this.cosmosClient = cosmosClient;
            this.containerName = containerName;
        }

        public async Task<List<T>> GetAll()
        {
            Container container = GetContainer();
            IQueryable<T> queryable = container.GetItemLinqQueryable<T>();

            List<T> result = new List<T>();
            FeedIterator<T> feedIterator = queryable.ToFeedIterator<T>();
            while (feedIterator.HasMoreResults)
            {
                FeedResponse<T> items = await feedIterator.ReadNextAsync();
                result.AddRange(items);
            }
            return result;
        }

        public async Task<ItemResponse<T>> CreateOrUpdate(T entity)
        {
            Container container = GetContainer();
            return await container.UpsertItemAsync<T>(entity, new PartitionKey(entity.PartitionKey));
        }
    }
}