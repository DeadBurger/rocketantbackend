using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

public static class SignalRFunctions
{
    [FunctionName("negotiate")]
    public static SignalRConnectionInfo GetSignalRInfo(
        [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req,
        [SignalRConnectionInfo(HubName = "taskhub", UserId = "{headers.x-ms-signalr-userid}")] SignalRConnectionInfo connectionInfo)
    {
        return connectionInfo;
    }
}