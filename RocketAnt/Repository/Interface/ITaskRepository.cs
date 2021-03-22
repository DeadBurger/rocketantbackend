using System.Threading.Tasks;

namespace RocketAnt.Repository
{
    public interface ITaskRepository : ICosmosRepository<BackgroundTask>
    {
        Task<BackgroundTask> GetRandomIncompleteTask();
    }
}