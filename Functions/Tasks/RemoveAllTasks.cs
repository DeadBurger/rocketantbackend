using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace RocketAnt.Function
{
    public class RemoveAllTasks
    {
        private readonly ITaskRepository taskRepository;

        public RemoveAllTasks(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }


        [FunctionName("RemoveAllTasks")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "tasks")] HttpRequest req,
            ILogger log)
        {
            await taskRepository.RemoveAll();
            return new OkResult();
        }
    }
}

