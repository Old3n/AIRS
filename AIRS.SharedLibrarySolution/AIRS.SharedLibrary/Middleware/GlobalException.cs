using AIRS.SharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AIRS.SharedLibrary.Middleware;
public class GlobalException(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        string title = "Internal Server Error";
        string message = "Internal server Error occuered";
        int statusCode = StatusCodes.Status500InternalServerError;

        try
        {

            await next(context);
            if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
            {
                title = "Too Many Request";
                message = "Too many requests made";
                statusCode = StatusCodes.Status429TooManyRequests;
            }
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                message = "Resource not found";
                title = "Not Found";
                statusCode = StatusCodes.Status404NotFound;
                await ModifyHeader(context, title, message, statusCode);
            }
            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                message = "Unauthorized access";
                title = "You are not authorized to access this resource";
                statusCode = StatusCodes.Status401Unauthorized;
                await ModifyHeader(context, title, message, statusCode);
            }
            if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                message = "Forbidden";
                title = "You are not allowed to access this resource";
                statusCode = StatusCodes.Status403Forbidden;
                await ModifyHeader(context, title, message, statusCode);
            }
        }
        catch (Exception ex)
        {
            //log exceptions to file/debug/console
            LogExceptions.LogException(ex);
            if (ex is TaskCanceledException || ex is TimeoutException)
            {
                title = "Request Timeout";
                message = "Request has been timed out";
                statusCode = StatusCodes.Status408RequestTimeout;
            }
            await ModifyHeader(context, title, message, statusCode);
        }
    }

    private static async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
        {
            Detail = message,
            Title = title,
            Status = statusCode

        }), CancellationToken.None);
        return;

    }
}
