namespace UserManagementAPI.Middleware
{
    using System.Text;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log the request
            context.Request.EnableBuffering();

            var requestBody = string.Empty;
            if (context.Request.ContentLength > 0)
            {
                using var reader = new StreamReader(
                    context.Request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    leaveOpen: true);

                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            _logger.LogInformation(
                "HTTP Request: {method} {path} | Body: {body}",
                context.Request.Method,
                context.Request.Path,
                requestBody);

            // Capture the response
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation(
                "HTTP Response: {statusCode} | Body: {body}",
                context.Response.StatusCode,
                responseText);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

}
