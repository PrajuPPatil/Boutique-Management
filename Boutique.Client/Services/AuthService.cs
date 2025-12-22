using System.Net.Http.Json;
using System.Threading.Tasks;
using Boutique.Client.Models;
using Boutique.Client.Models.DTOs;
using Boutique.Client.Models.DTOs.Authentication;
using System.Text.Json.Serialization;

namespace Boutique.Client.Services
{
    // Authentication service for handling user login, registration, and password operations
    public class AuthService
    {
        // HTTP client for API communication with authentication endpoints
        private readonly HttpClient _httpClient;

        // Constructor with dependency injection for HTTP client
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Authenticate user with email and password, returns JWT token on success
        public async Task<LoginResponseDto?> LoginAsync(LoginDTO loginDto)
        {
            try
            {
                // Send login request to backend API
                var response = await _httpClient.PostAsJsonAsync("api/Auth/login", loginDto);
                
                if (response.IsSuccessStatusCode)
                {
                    // Return login response containing JWT token and user info
                    return await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                }
                
                // Handle login errors with detailed messages
                var errorContent = await response.Content.ReadAsStringAsync();
                
                // Try to parse error message from JSON response
                try
                {
                    var errorResponse = System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(errorContent);
                    var errorMessage = errorResponse?.Message ?? "Login failed. Please try again.";
                    throw new HttpRequestException(errorMessage);
                }
                catch (System.Text.Json.JsonException)
                {
                    // If not JSON, use the raw error content
                    throw new HttpRequestException(errorContent.Length > 200 ? "Login failed. Please check your credentials." : errorContent);
                }
            }
            catch (HttpRequestException)
            {
                // Re-throw HTTP exceptions (these contain our user-friendly messages)
                throw;
            }
            catch (Exception)
            {
                // Handle network or other unexpected errors
                throw new HttpRequestException("Unable to connect to the server. Please check your internet connection and try again.");
            }
        }

        // Register new user account with email verification
        public async Task<string> RegisterAsync(RegisterDTO registerDto)
        {
            try
            {
                // Send registration request to backend API
                var response = await _httpClient.PostAsJsonAsync("api/Auth/register", registerDto);
                
                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                    return successResponse?.Message ?? "Registration successful! Please check your email for verification.";
                }
                
                // Handle registration errors with detailed error messages
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                var errorMessage = errorResponse?.Message ?? "Registration failed. Please try again.";
                
                // Throw exception with user-friendly error message
                throw new HttpRequestException(errorMessage);
            }
            catch (HttpRequestException)
            {
                // Re-throw HTTP exceptions (these contain our user-friendly messages)
                throw;
            }
            catch (Exception)
            {
                // Handle network or other unexpected errors
                throw new HttpRequestException("Unable to connect to the server. Please check your internet connection and try again.");
            }
        }
        
        // Helper class for API responses
        public class ApiResponse
        {
            public string Message { get; set; } = string.Empty;
            public bool Success { get; set; }
        }

        // Logout user and invalidate session on server
        public async Task LogoutAsync()
        {
            // Send logout request to backend API
            await _httpClient.PostAsync("api/Auth/logout", null);
        }

        // Verify OTP code for email confirmation
        public async Task VerifyOtpAsync(OtpVerifyDTO otpDto)
        {
            // Send OTP verification request to backend API
            var response = await _httpClient.PostAsJsonAsync("api/Auth/verify-otp", otpDto);
            response.EnsureSuccessStatusCode();
        }

        // Resend OTP code to user's email address
        public async Task ResendOtpAsync(string email)
        {
            // Send resend OTP request to backend API
            var response = await _httpClient.PostAsJsonAsync("api/Auth/resend-otp", new { Email = email });
            response.EnsureSuccessStatusCode();
        }

        // Initiate password reset process by sending reset email
        public async Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            // Send forgot password request to backend API
            var response = await _httpClient.PostAsJsonAsync("api/Auth/forgot-password", forgotPasswordDto);
            response.EnsureSuccessStatusCode();
        }
    }
}
