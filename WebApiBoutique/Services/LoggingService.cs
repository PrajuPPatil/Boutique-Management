namespace WebApiBoutique.Services
{
    public class LoggingService
    {
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public void LogInfo(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        public void LogError(Exception ex, string message, params object[] args)
        {
            _logger.LogError(ex, message, args);
        }

        public void LogDebug(string message, params object[] args)
        {
            _logger.LogDebug(message, args);
        }

        public void LogUserAction(string userId, string action, string details = "")
        {
            _logger.LogInformation("User {UserId} performed {Action}. Details: {Details}", 
                userId, action, details);
        }

        public void LogApiCall(string endpoint, string method, int statusCode)
        {
            _logger.LogInformation("API Call: {Method} {Endpoint} returned {StatusCode}", 
                method, endpoint, statusCode);
        }
    }
}