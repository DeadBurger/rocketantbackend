using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;

public class TestFactory
{
    private static Dictionary<string, StringValues> CreateDictionary(string key, string value)
    {
        var qs = new Dictionary<string, StringValues>
            {
                { key, value }
            };
        return qs;
    }

    public static HttpRequest CreateHttpRequest(string queryStringKey, string queryStringValue)
    {
        var context = new DefaultHttpContext();
        var request = context.Request;
        request.Query = new QueryCollection(CreateDictionary(queryStringKey, queryStringValue));
        return request;
    }

    public static ILogger CreateLogger()
    {
        return NullLoggerFactory.Instance.CreateLogger("Null Logger");
    }
}