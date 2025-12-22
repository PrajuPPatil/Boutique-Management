using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;

namespace Boutique.Client.Services
{
    // Custom authentication state provider for JWT token-based authentication
    // Manages user authentication state and token persistence in local storage
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        // Local storage service for JWT token persistence
        private readonly ILocalStorageService _localStorage;
        // HTTP client for setting authorization headers
        private readonly HttpClient _httpClient;

        // Constructor with dependency injection for local storage and HTTP client
        public CustomAuthenticationStateProvider(
            ILocalStorageService localStorage,
            HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        // Get current authentication state from stored JWT token
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Retrieve JWT token from local storage
            var token = await _localStorage.GetItemAsync<string>("authToken");

            // Return anonymous user if no token found
            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            try
            {
                // Parse claims from JWT token
                var claims = ParseClaimsFromJwt(token);
                var identity = new ClaimsIdentity(claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                // Set authorization header for API requests
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                return new AuthenticationState(user);
            }
            catch
            {
                // Return anonymous user if token parsing fails
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        // Mark user as authenticated after successful login
        public async Task MarkUserAsAuthenticated(string token)
        {
            // Store JWT token in local storage for persistence
            await _localStorage.SetItemAsync("authToken", token);

            // Parse user claims from JWT token
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            // Set authorization header for future API requests
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Notify all components about authentication state change
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        // Mark user as logged out and clear authentication data
        public async Task MarkUserAsLoggedOut()
        {
            // Remove JWT token and user data from local storage
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("currentUser");

            // Clear authorization header from HTTP client
            _httpClient.DefaultRequestHeaders.Authorization = null;

            // Create anonymous user identity
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);

            // Notify all components about logout
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        // Parse user claims from JWT token for authentication state
        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            // Use JWT handler to parse token and extract claims
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            return token.Claims;
        }
    }
}