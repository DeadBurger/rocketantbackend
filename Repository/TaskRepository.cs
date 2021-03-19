using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace RocketAnt.Function
{
    public class TaskRepository : CosmosRepository<BackgroundTask>, ITaskRepository
    {
        public TaskRepository(CosmosClient cosmosClient) : base(cosmosClient, "tasks")
        {
        }

        public async Task<BackgroundTask> GetRandomIncompleteTask()
        {
            Container container = GetContainer();
            int rowIndex = await GetRandomRowIndex(container);

            if (rowIndex == -1)
                return null;

            IQueryable<BackgroundTask> queryable = container.GetItemLinqQueryable<BackgroundTask>()
            .Where(o => !o.IsCompleted);

            FeedIterator<BackgroundTask> feedIterator = queryable.ToFeedIterator<BackgroundTask>();


            List<BackgroundTask> result = new List<BackgroundTask>();
            while (feedIterator.HasMoreResults)
            {
                FeedResponse<BackgroundTask> items = await feedIterator.ReadNextAsync();
                result.AddRange(items);

                if (result.Count > rowIndex)
                    return result[rowIndex];
            }
            return null;
        }

        private static async Task<int> GetRandomRowIndex(Container container)
        {
            int count = await container.GetItemLinqQueryable<BackgroundTask>()
            .Where(o => !o.IsCompleted)
            .CountAsync();

            if (count == 0)
                return -1;

            Random random = new Random();

            int rowIndex = random.Next(0, count - 1);
            return rowIndex;
        }
    }
}