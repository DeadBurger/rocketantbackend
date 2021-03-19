namespace RocketAnt.Function
{
    public class TaskContract
    {
        public string Id { get; set; }

        public int NumOfSteps { get; set; }

        public int CurrentStep { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}