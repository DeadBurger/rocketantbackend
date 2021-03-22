using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using RocketAnt.Contract;
using RocketAnt.Repository;

namespace RocketAnt.Function
{
    public class GetLatestTasks
    {
        private readonly ITaskRepository taskRepository;

        public GetLatestTasks(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        [FunctionName("GetLatestTasks")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tasks/latest")] HttpRequest req,
            ILogger log)
        {
            var tasks = await taskRepository.GetLatest(20);

            var result = tasks.Select(o => new TaskContract()
            {
                Id = o.Id,
                CurrentStep = o.CurrentStep,
                Description = o.Description,
                NumOfSteps = o.NumOfSteps,
                IsCompleted = o.IsCompleted
            }).ToList();

            return new OkObjectResult(result);
        }
    }
}

