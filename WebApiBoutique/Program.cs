// Main entry point for Boutique Management System Web API
// Configures services, middleware, authentication, and database connections
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApiBoutique.Auth;
using WebApiBoutique.Repository;
using WebApiBoutique.Repository.Interface;
using WebApiBoutique.Services;
using WebApiBoutique.Services.Interface;
using System.Text.Json.Serialization;
using WebApiBoutique.Data;
using WebApiBoutique.Middleware;

// Create web application builder
var builder = WebApplication.CreateBuilder(args);

// JWT Configuration - Load JWT settings from appsettings.json
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
// Validate JWT configuration exists
if (jwt == null || string.IsNullOrEmpty(jwt.Key))
{
    throw new InvalidOperationException("JWT configuration is missing or invalid.");
}
// Create symmetric security key for JWT token signing
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));

// Database Context - Configure SQL Server connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Validate connection string exists
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Database connection string is missing.");
}

// Register database context with SQL Server provider
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Dependency Injection - Register services and repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();                           // User data access
builder.Services.AddScoped<IAuthService, AuthService>();                                 // Authentication logic
builder.Services.AddScoped<IEmailService, EmailService>();                               // Email sending
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();                         // JWT token generation
builder.Services.AddScoped<ICustomerService, CustomerService>();                         // Customer operations
builder.Services.AddScoped<IUserService, UserService>();                                 // User management
builder.Services.AddScoped<IPaymentService, PaymentService>();                           // Payment processing
builder.Services.AddScoped<IMeasurementService, MeasurementService>();                   // Measurement operations
builder.Services.AddScoped<IMeasurementValidationService, MeasurementValidationService>(); // Measurement validation
builder.Services.AddScoped<IOrderService, OrderService>();                               // Order management
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();                       // Business analytics
builder.Services.AddScoped<SeedDataService>();                                           // Database seeding
builder.Services.AddSingleton<ConfigurationService>();                                   // Configuration helper
builder.Services.AddScoped<ValidationService>();                                         // Validation helper
builder.Services.AddScoped<LoggingService>();                                            // Logging service

// JWT Authentication - Configure JWT Bearer authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Configure token validation parameters for security
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,              // Validate token issuer
            ValidateAudience = true,            // Validate token audience
            ValidateLifetime = true,            // Check token expiration
            ValidateIssuerSigningKey = true,    // Validate signing key
            ValidIssuer = jwt.Issuer,           // Expected issuer
            ValidAudience = jwt.Audience,       // Expected audience
            IssuerSigningKey = key,             // Signing key for validation
            ClockSkew = TimeSpan.Zero           // No clock skew tolerance
        };
    });

// Add authorization services
builder.Services.AddAuthorization();

// CORS Policy - Configure Cross-Origin Resource Sharing (must be before controllers)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()   // Allow requests from any origin (for development)
              .AllowAnyMethod()   // Allow all HTTP methods (GET, POST, PUT, DELETE)
              .AllowAnyHeader();  // Allow all headers
    });
});

// API Controllers & Swagger - Configure API controllers and documentation
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Ignore circular references in JSON serialization
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        // Format JSON output for readability
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Add API explorer for Swagger
builder.Services.AddEndpointsApiExplorer();
// Configure Swagger documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "WebAPI Boutique", Version = "v1" });
});
// Add HTTP context accessor for accessing request context
builder.Services.AddHttpContextAccessor();

// Logging - Configure application logging
builder.Logging.ClearProviders();  // Clear default logging providers
builder.Logging.AddConsole();      // Add console logging
builder.Logging.AddDebug();        // Add debug logging

// Build the application
var app = builder.Build();

// Middleware Pipeline - Configure request processing pipeline (order is critical!)
// Global exception handler for consistent error responses
app.UseMiddleware<GlobalExceptionHandler>();

// Development-only middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();  // Detailed error pages
    app.UseSwagger();                 // Swagger JSON endpoint
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI Boutique v1"));  // Swagger UI
}

// HTTPS redirection for security (only in production)
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
// CORS must be before Authentication
app.UseCors("AllowAll");
// Authentication middleware (validates JWT tokens)
app.UseAuthentication();
// Authorization middleware (checks user permissions)
app.UseAuthorization();
// Map controller endpoints
app.MapControllers();

// Seed initial data
using (var scope = app.Services.CreateScope())
{
    var seedService = scope.ServiceProvider.GetRequiredService<SeedDataService>();
    await seedService.SeedAsync();
}

// Start the application
app.Run();
