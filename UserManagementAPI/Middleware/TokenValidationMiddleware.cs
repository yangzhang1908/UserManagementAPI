using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace UserManagementAPI.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ValidToken = "YOUR_SUPER_SECRET_TOKEN"; // Replace with actual token validation logic

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip authentication for Swagger UI paths in development
            if (context.Request.Path.StartsWithSegments("/swagger") ||
                context.Request.Path.StartsWithSegments("/api/swagger")) // Adjust if Swagger is mapped differently
            {
                await _next(context);
                return;
            }

            // Check for Authorization header
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Authorization token missing.");
                return;
            }

            string authorizationHeader = context.Request.Headers["Authorization"];
            if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid Authorization header format. Must be 'Bearer [token]'.");
                return;
            }

            string token = authorizationHeader.Substring("Bearer ".Length).Trim();

            if (token != ValidToken) // Simplified validation
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid token.");
                return;
            }

            // If token is valid, proceed to the next middleware
            await _next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class TokenValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenValidationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenValidationMiddleware>();
        }
    }
}
