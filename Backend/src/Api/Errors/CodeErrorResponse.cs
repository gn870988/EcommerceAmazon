using Newtonsoft.Json;

namespace Ecommerce.Api.Errors;

public class CodeErrorResponse
{
    [JsonProperty(PropertyName = "statusCode")]
    public int StatusCode { get; set; }

    [JsonProperty(PropertyName = "message")]
    public string[]? Message { get; set; }

    public CodeErrorResponse(int statusCode, string[]? message = null)
    {
        StatusCode = statusCode;

        if (message is null)
        {
            Message = Array.Empty<string>();
            var text = GetDefaultMessageStatusCode(statusCode);
            Message[0] = text;
        }
        else
        {
            Message = message;
        }

    }

    private string GetDefaultMessageStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "The Request sent has errors",
            401 => "You do not have authorization for this resource",
            404 => "The requested resource was not found",
            500 => "Server errors occurred",
            _ => string.Empty
        };
    }
}