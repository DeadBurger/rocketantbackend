namespace RocketAnt.Function
{
    public class TaskCreatedMapper : IMapper<TaskCreatedContract, BackgroundTask>
    {
        public TaskCreatedContract Map(BackgroundTask map)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IMapper<T, S>
    {
        public abstract T Map(S map);
    }

}