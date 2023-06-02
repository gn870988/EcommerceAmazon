using Ecommerce.Api.Errors;
using Ecommerce.Application.Exceptions;
using Ecommerce.Infrastructure.Extensions;
using Newtonsoft.Json;
using System.Net;

namespace Ecommerce.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            context.Response.ContentType = "application/json";
            int statusCode;
            var result = string.Empty;

            switch (ex)
            {
                case NotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    break;

                case FluentValidation.ValidationException validationException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    var errors = validationException.Errors.Select(ers => ers.ErrorMessage).ToArray();
                    var validationJson = JsonConvert.SerializeObject(errors);
                    result = JsonConvert.SerializeObject(
                        new CodeErrorException(statusCode, errors, validationJson)
                    );
                    break;

                case BadRequestException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            if (result.IsNullOrEmpty())
            {
                result = JsonConvert.SerializeObject(
                    new CodeErrorException(statusCode,
                        new[] { ex.Message }, ex.StackTrace));
            }

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(result);
        }
    }
}