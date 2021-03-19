using System.Threading.Tasks;

namespace RocketAnt.Function
{
    public interface ITaskRepository : ICosmosRepository<BackgroundTask>
    {
        Task<BackgroundTask> GetRandomIncompleteTask();
    }
}