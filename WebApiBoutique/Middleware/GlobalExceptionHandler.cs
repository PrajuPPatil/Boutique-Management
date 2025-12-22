using System.Net;
using System.Text.Json;

namespace WebApiBoutique.Middleware
{
    // Global exception handling middleware for consistent error responses
    public class GlobalExceptionHandler
    {
        // Next middleware in the pipeline
        private readonly RequestDelegate _next;
        // Logger for recording exception details
        private readonly ILogger<GlobalExceptionHandler> _logger;

        // Constructor to initialize middleware dependencies
        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        // Main middleware execution method
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Continue to next middleware in pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception with full details for debugging
                _logger.LogError(ex, "An unhandled exception occurred");
                // Handle the exception and return appropriate response
                await HandleExceptionAsync(context, ex);
            }
        }

        // Handle exceptions and return appropriate HTTP responses
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Set response content type to JSON
            context.Response.ContentType = "application/json";
            
            // Create standardized error response
            var response = new ErrorResponse
            {
                Message = "An error occurred while processing your request",
                Details = exception.Message
            };

            // Map exception types to appropriate HTTP status codes
            context.Response.StatusCode = exception switch
            {
                InvalidOperationException => (int)HttpStatusCode.BadRequest,      // 400
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,   // 401
                KeyNotFoundException => (int)HttpStatusCode.NotFound,              // 404
                _ => (int)HttpStatusCode.InternalServerError                       // 500
            };

            // Hide internal error details from clients for security
            if (context.Response.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                response.Details = "Please contact support if the problem persists";
            }

            // Serialize error response to JSON and return to client
            var result = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(result);
        }
    }

    // Standardized error response model for consistent API error format
    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;  // User-friendly error message
        public string Details { get; set; } = string.Empty;  // Additional error details
    }
}
