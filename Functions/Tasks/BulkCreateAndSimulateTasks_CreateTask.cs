using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;

namespace RocketAnt.Function
{
    public class BulkCreateAndSimulateTasks_CreateTask
    {
        private readonly ITaskRepository taskRepository;
        public BulkCreateAndSimulateTasks_CreateTask(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        [FunctionName("BulkCreateAndSimulateTasks_CreateTask")]
        public async Task<bool> Run(
            [ActivityTrigger] IDurableActivityContext context,
            [SignalR(HubName = "taskhub")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            Random random = new Random();

            BackgroundTask backgroundTask = new BackgroundTask();
            backgroundTask.CustomerId = "testcustomer";
            backgroundTask.NumOfSteps = random.Next(2, 8);
            backgroundTask.Id = Guid.NewGuid().ToString();
            backgroundTask.Description = "test task " + backgroundTask.Id;

            ItemResponse<BackgroundTask> createResult = await taskRepository.CreateOrUpdate(backgroundTask);
            await SendSignalRMessage(signalRMessages, backgroundTask);

            TaskCreatedContract result = new TaskCreatedContract()
            {
                Id = backgroundTask.Id
            };

            return true;
        }

        private static async Task SendSignalRMessage(IAsyncCollector<SignalRMessage> signalRMessages, BackgroundTask backgroundTask)
        {
            await signalRMessages.AddAsync(new SignalRMessage()
            {
                Target = "taskCreated",
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

