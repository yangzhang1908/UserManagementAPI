using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace UserManagementAPI.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log incoming request details
            _logger.LogInformation(
                "Request: {Method} {Path}",
                context.Request.Method,
                context.Request.Path
            );

            // Call the next middleware in the pipeline
            await _next(context);

            // Log outgoing response details
            _logger.LogInformation(
                "Response: {Method} {Path} Status Code: {StatusCode}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode
            );
        }
    }

    // Extension method for easier registration in Program.cs
    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
