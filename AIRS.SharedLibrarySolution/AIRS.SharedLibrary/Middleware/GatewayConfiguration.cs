using Microsoft.AspNetCore.Http;

namespace AIRS.SharedLibrary.Middleware;
public class GatewayConfiguration(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value ?? string.Empty;
        if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/v3/api-docs", StringComparison.OrdinalIgnoreCase))
        {
            await next(context);
            return;
        }
        var signedHeader = context.Request.Headers["X-From-NginxGateWay"];
        if (signedHeader.FirstOrDefault() is null)
        {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await context.Response.WriteAsync("Service Unavailable");
            return;
        }
        else
        {
            await next(context);
        }
    }
}
