using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

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
            ILogger log)
        {
            var task = await taskRepository.GetRandomIncompleteTask();
            task.NextStep();
            await this.taskRepository.CreateOrUpdate(task);
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
