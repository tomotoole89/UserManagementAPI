namespace UserManagementAPI.Middleware
{
    using System.Net;
    using Microsoft.AspNetCore.Http;

    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;

            if (path.StartsWith("/swagger") || path.StartsWith("/favicon"))
            {
                await _next(context);
                return;
            }

            // Extract token from Authorization header
            var authHeader = context.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                await WriteUnauthorizedResponse(context);
                return;
            }

            var token = authHeader["Bearer ".Length..].Trim();

            // Validate the token (replace with your real validation logic)
            if (!IsValidToken(token))
            {
                await WriteUnauthorizedResponse(context);
                return;
            }

            // Token is valid → continue pipeline
            await _next(context);
        }

        private bool IsValidToken(string token)
        {
            // Example: simple hardcoded token check
            // Replace with JWT validation, database lookup, etc.
            return token == "my-secret-token";
        }

        private async Task WriteUnauthorizedResponse(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                error = "Unauthorized. Invalid or missing token."
            });
        }
    }

}
