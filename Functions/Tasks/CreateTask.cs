using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;

namespace RocketAnt.Function
{
    public class CreateTask
    {
        private readonly ITaskRepository taskRepository;
        private readonly IContractValidator<CreateTaskContract> contractValidator;

        public CreateTask(ITaskRepository taskRepository, IContractValidator<CreateTaskContract> contractValidator)
        {
            this.taskRepository = taskRepository;
            this.contractValidator = contractValidator;
        }

        [FunctionName("CreateTask")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tasks")] CreateTaskContract contract,
            [SignalR(HubName = "taskhub")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            var validationResult = contractValidator.Validate(contract);
            if (validationResult.Errors.Any())
            {
                return new BadRequestObjectResult(validationResult.Errors.First().Error);
            }

            BackgroundTask backgroundTask = new BackgroundTask();
            backgroundTask.CustomerId = "testcustomer";
            backgroundTask.NumOfSteps = contract.NumOfSteps.Value;
            backgroundTask.Id = Guid.NewGuid().ToString();
            backgroundTask.Description = "test task " + backgroundTask.Id;

            ItemResponse<BackgroundTask> createResult = await taskRepository.CreateOrUpdate(backgroundTask);
            await SendSignalRMessage(signalRMessages, backgroundTask);

            TaskCreatedContract result = new TaskCreatedContract()
            {
                Id = backgroundTask.Id
            };

            return new OkObjectResult(result);
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
                        Desciption = backgroundTask.Description
                    }
                }
            });
        }
    }
}

