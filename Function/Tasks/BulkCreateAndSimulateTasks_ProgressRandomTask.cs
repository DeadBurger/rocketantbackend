using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using RocketAnt.Repository;
using RocketAnt.Contract;

namespace RocketAnt.Function
{
    public class BulkCreateAndSimulateTasks_ProgressRandomTask
    {
        private readonly ITaskRepository taskRepository;

        public BulkCreateAndSimulateTasks_ProgressRandomTask(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        [FunctionName("BulkCreateAndSimulateTasks_ProgressRandomTask")]
        public async Task<bool> Run(
            [ActivityTrigger] IDurableActivityContext context,
            [SignalR(HubName = "taskhub")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger logger)
        {
            var task = await taskRepository.GetRandomIncompleteTask();
            if (task == null)
                return false;

            task.NextStep();
            await this.taskRepository.CreateOrUpdate(task);
            await SendSignalRMessage(signalRMessages, task);
            return true;
        }

        private static async Task SendSignalRMessage(IAsyncCollector<SignalRMessage> signalRMessages, BackgroundTask backgroundTask)
        {
            await signalRMessages.AddAsync(new SignalRMessage()
            {
                Target = "taskUpdated",
                Arguments = new[] {
                    new TaskContract()
                    {
                        Id = backgroundTask.Id,
                        CurrentStep = backgroundTask.CurrentStep,
                        NumOfSteps = backgroundTask.NumOfSteps,
                        IsCompleted = backgroundTask.IsCompleted,
                        Description = backgroundTask.Description
                    }
                }
            });
        }
    }
}
