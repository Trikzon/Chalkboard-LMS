using System.Net;
using System.Runtime.CompilerServices;
using Refit;

namespace Lms.Library.Api.WebApi;

public static class ApiUtils
{
    public const string HostUrl = "http://localhost:5169";

    public static void HandleApiException(ApiException exception, [CallerMemberName] string functionName = "")
    {
        switch (exception.StatusCode)
        {
            case HttpStatusCode.NotFound:
                Console.WriteLine($"[{functionName}] Resource not found.");
                break;
            case HttpStatusCode.BadRequest:
            case HttpStatusCode.MethodNotAllowed:
                Console.WriteLine($"[{functionName}] Bad request.");
                break;
            default:
                Console.WriteLine($"[{functionName}] An error occurred: {exception.Message}");
                break;
        }
    }
}