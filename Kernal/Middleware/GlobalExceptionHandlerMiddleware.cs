using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace SharedKernel;
public class GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
{
    public async Task InvokeAsync(Microsoft.AspNetCore.Http.HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    private async Task HandleExceptionAsync(Microsoft.AspNetCore.Http.HttpContext context, Exception exception)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;

        int statusCode;
        object response;

        if (exception is ProblemException customException)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
            response =
                new
                {
                    Status = ResultStatus.CriticalError.ToString(),
                    Code = ExceptionCodes.Infrastructure,
                    Message = customException.Exception.Message ?? ResponseMessages.Failure,
                    InnerExceptionMessage = customException.InnerException?.Message ?? null,
                    IsSuccess = false,
                    StackTrace = FormatStackTrace(customException.Exception.StackTrace ?? customException.Exception.InnerException?.StackTrace ?? null)
                }; 
        }
        else
        {
            statusCode = (int)HttpStatusCode.InternalServerError;
            response =
                new { 
                    Status = ResultStatus.CriticalError.ToString(),
                    Code = ExceptionCodes.Infrastructure ,
                    Message = exception.Message ?? ResponseMessages.Failure,
                    InnerExceptionMessage = exception.InnerException?.Message ?? null,                    
                    IsSuccess = false,
                    StackTrace = FormatStackTrace(exception.StackTrace ?? exception.InnerException?.StackTrace ?? null)
                };            
        }

        logger.LogError(exception, ResponseMessages.Failure);

        var json = JsonSerializer.Serialize(response);

        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(json);
    }

    private List<string?>? FormatStackTrace(string stackTrace)
    {
        if (string.IsNullOrWhiteSpace(stackTrace))
            return [];

        
        var lines = stackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        return lines
            .Select(line =>
            {
                var startIndex = line.LastIndexOf(" in ", StringComparison.Ordinal);
                if (startIndex > -1)
                {
                    var endIndex = line.LastIndexOf(":line ", StringComparison.Ordinal);
                    if (endIndex > -1)
                    {
                        var className = ExtractClassName(line);
                        var lineNumber = line[( endIndex + 6 )..];
                        return $"{className}, Line {lineNumber}";
                    }
                }
                return null;
            })
            .Where(result => result != null)
            .ToList();
    }

    private static string ExtractClassName(string line)
    {
        var atIndex = line.IndexOf("at ", StringComparison.Ordinal);
        if (atIndex == -1) return "Unknown";

        var start = atIndex + 3;
        var end = line.IndexOf('(', start);
        if (end == -1) return "Unknown";

        var classWithMethod = line[start..end].Trim();
        var lastDotIndex = classWithMethod.LastIndexOf('.');
        return lastDotIndex > -1 ? classWithMethod[..lastDotIndex] : classWithMethod;
    }
}