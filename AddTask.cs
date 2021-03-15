using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace RocketAnt.Function
{
    public class AddTask
    {
        [FunctionName("AddTask")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("SendMessage")]
        public static async Task<IActionResult> SendMessage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [SignalR(HubName = "SignalRTest")] IAsyncCollector<SignalRMessage> signalRMessages, ILogger log)
        {
            string name = req.Query["name"];

            await signalRMessages.AddAsync(
                        new SignalRMessage
                        {
                            Target = "SendMessage",
                            Arguments = new[] { name
                            }
                        });

            await signalRMessages.FlushAsync();

            return new OkObjectResult("OK");
        }

    }
}

