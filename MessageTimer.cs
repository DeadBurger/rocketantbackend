using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace RocketAnt.Function
{
    public static class MessageTimer
    {
        [FunctionName("MessageTimer")]
        public static Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, [SignalR(HubName = "taskhub")] IAsyncCollector<SignalRMessage> signalRMessages, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            return signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "taskUpdate",
                    Arguments = new[] { "new message testing" }
                });
        }
    }

}
