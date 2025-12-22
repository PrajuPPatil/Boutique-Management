// Main entry point for Boutique Management System Blazor WebAssembly client
// Configures services, authentication, and dependency injection
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Boutique.Client;
using Boutique.Client.Services;

using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using System;
using System.Net.Http;

// Create WebAssembly host builder
var builder = WebAssemblyHostBuilder.CreateDefault(args);
// Register root components for Blazor app
builder.RootComponents.Add<App>("#app");           // Main app component
builder.RootComponents.Add<HeadOutlet>("head::after"); // Head content management

// HTTP Client Configuration - Setup authenticated API communication
// Register custom authorization message handler for JWT token injection
builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

// Register named HttpClient with authentication for API calls
builder.Services.AddHttpClient("API", client => 
{
    client.BaseAddress = new Uri("http://localhost:5100/"); // Backend API URL
})
.AddHttpMessageHandler<CustomAuthorizationMessageHandler>(); // Add JWT token to requests

// Register default HttpClient for non-authenticated calls (fallback)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5100/") });

// Application Services Registration - Register all business logic services
// Authentication and user management services
builder.Services.AddScoped<AuthService>(sp => 
    new AuthService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("API")));
// Customer management service
builder.Services.AddScoped<CustomerService>(sp => 
    new CustomerService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("API")));
// Payment processing service
builder.Services.AddScoped<PaymentService>(sp => 
    new PaymentService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("API")));
// Measurement management service
builder.Services.AddScoped<MeasurementService>(sp => 
    new MeasurementService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("API")));
// Customer measurement operations service
builder.Services.AddScoped<CustomerMeasurementService>(sp => 
    new CustomerMeasurementService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("API")));
// Dashboard data service
builder.Services.AddScoped<DashboardService>(sp => 
    new DashboardService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("API")));
// Order management service
builder.Services.AddScoped<OrderService>(sp => 
    new OrderService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("API")));
// Global search service
builder.Services.AddScoped<SearchService>(sp => 
    new SearchService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("API")));
// Client-side notification service
builder.Services.AddScoped<NotificationService>();
// Analytics and reporting service
builder.Services.AddScoped<AnalyticsService>(sp => 
    new AnalyticsService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("API")));
// Data export service
builder.Services.AddScoped<ExportService>();
// User profile service
builder.Services.AddScoped<UserService>();
// Profile update service
builder.Services.AddScoped<ProfileUpdateService>();
// JWT token handling service
builder.Services.AddScoped<JwtTokenService>();
// UI theme management service for dark/light mode switching
builder.Services.AddScoped<ThemeService>();
// Garment type management service - handles CRUD operations for garment types
// Uses authenticated HTTP client for API communication
builder.Services.AddScoped<TypeService>(sp => 
    new TypeService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("API")));


// Local Storage Configuration - For JWT token persistence
builder.Services.AddBlazoredLocalStorage();

// Authentication & Authorization Setup - Configure security services
builder.Services.AddAuthorizationCore();  // Core authorization services
// Custom authentication state provider for JWT handling
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
// Register as authentication state provider interface
builder.Services.AddScoped<AuthenticationStateProvider>(provider => 
    provider.GetRequiredService<CustomAuthenticationStateProvider>());

// Build and run the Blazor WebAssembly application
await builder.Build().RunAsync();
