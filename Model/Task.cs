
namespace RocketAnt.Function
{
    public class BackgroundTask : CosmosEntity
    {
        public string CustomerId { get; set; }

        public override string PartitionKey => CustomerId;

        public int CurrentStep { get; protected set; } = 1;
        public int NumOfSteps { get; set; }

        public void NextStep()
        {
            if (CurrentStep + 1 <= NumOfSteps)
                CurrentStep++;
        }

        public bool IsCompleted => CurrentStep == NumOfSteps;
    }
}