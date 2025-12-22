namespace WebApiBoutique.Services
{
    public class ConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString() => 
            _configuration.GetConnectionString("DefaultConnection") ?? 
            throw new InvalidOperationException("Database connection string not found");

        public string GetJwtKey() => 
            _configuration["Jwt:Key"] ?? 
            throw new InvalidOperationException("JWT key not configured");

        public string GetEmailPassword() => 
            _configuration["Email:SenderPassword"] ?? "";

        public bool IsProduction() => 
            _configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") == "Production";

        public string GetApiBaseUrl() => 
            _configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5100";

        public int GetTokenExpiryMinutes() => 
            _configuration.GetValue<int>("Jwt:ExpiryMinutes", 60);
    }
}