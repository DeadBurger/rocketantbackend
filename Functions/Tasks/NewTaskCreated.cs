using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace RocketAnt.Function
{
    public class NewTaskCreated
    {
        [FunctionName("NewTaskCreated")]
        public async Task Run([CosmosDBTrigger(
            databaseName: "rocketant",
            collectionName: "tasks",
            ConnectionStringSetting = "CosmosDBConnection",
            LeaseCollectionName = "leases",
            LeaseCollectionPrefix="newtask")] IReadOnlyList<Document> input,
            [SignalR(HubName = "taskhub")] IAsyncCollector<SignalRMessage> signalRMessages, ILogger log)
        {

            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
            }

            await signalRMessages.AddAsync(
            new SignalRMessage
            {
                Target = "taskUpdate",
                Arguments = new[] { "new message testing" }
            });
        }
    }
}
