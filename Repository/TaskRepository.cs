using Microsoft.Azure.Cosmos;

namespace RocketAnt.Function
{
    public class TaskRepository : CosmosRepository<BackgroundTask>, ITaskRepository
    {
        public TaskRepository(CosmosClient cosmosClient) : base(cosmosClient, "tasks")
        {
        }
    }
}