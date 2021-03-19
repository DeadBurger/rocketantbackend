using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace RocketAnt.Function
{
    public class BulkCreateAndSimulateTasks
    {
        [FunctionName("BulkCreateAndSimulateTasks")]
        public static async Task<List<bool>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var contract = context.GetInput<BulkCreateTasksContract>();

            var outputs = new List<bool>();

            List<Task<bool>> tasks = new List<Task<bool>>();

            foreach (var i in Enumerable.Range(1, contract.NumberOfTasks.Value))
            {
                tasks.Add(context.CallActivityAsync<bool>("BulkCreateAndSimulateTasks_CreateTask", null));
            }
            await Task.WhenAll(tasks.ToArray());

            await context.CallActivityAsync<bool>("BulkCreateAndSimulateTasks_ProgressRandomTask", null);
            while (await context.CallActivityAsync<bool>("BulkCreateAndSimulateTasks_ProgressRandomTask", null))
            {
                await context.CreateTimer(context.CurrentUtcDateTime.AddMilliseconds(300), CancellationToken.None);
            };

            return tasks.Select(o => o.Result).ToList();
        }


        [FunctionName("BulkCreateAndSimulateTasks_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tasks/bulkcreate")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            var contract = await req.Content.ReadAsAsync<BulkCreateTasksContract>();

            string instanceId = await starter.StartNewAsync("BulkCreateAndSimulateTasks", null, contract);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}