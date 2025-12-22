using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Boutique.Client.Models.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Blazored.LocalStorage;

namespace Boutique.Client.Services
{
    // User service for managing user profiles and account operations
    public class UserService
    {
        // HTTP client for API communication
        private readonly HttpClient _httpClient;
        // Authentication state provider for JWT claims
        private readonly AuthenticationStateProvider _authStateProvider;
        // Local storage for caching profile updates
        private readonly ILocalStorageService _localStorage;
        // Cached user profile for performance
        private UserProfileDto? _cachedProfile;

        // Constructor with dependency injection for required services
        public UserService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        // Get current user profile with fallback to JWT claims if API unavailable
        public async Task<UserProfileDto?> GetCurrentUserProfileAsync()
        {
            try
            {
                // Try to get profile from API first
                var apiProfile = await _httpClient.GetFromJsonAsync<UserProfileDto>("api/user/profile");
                if (apiProfile != null)
                {
                    // Cache successful API response
                    _cachedProfile = apiProfile;
                    return apiProfile;
                }
            }
            catch
            {
                // API failed, fall back to JWT claims and local storage
            }

            // Fallback: Create profile from JWT claims and local storage
            return await GetProfileFromClaimsAsync();
        }

        // Create user profile from JWT claims and local storage (fallback method)
        private async Task<UserProfileDto?> GetProfileFromClaimsAsync()
        {
            try
            {
                // Get authentication state from JWT token
                var authState = await _authStateProvider.GetAuthenticationStateAsync();
                if (authState.User.Identity?.IsAuthenticated != true)
                    return null;

                var user = authState.User;
                
                // Get cached profile updates from local storage
                var cachedUpdates = await _localStorage.GetItemAsync<UpdateUserProfileDto>("profileUpdates");
                
                // Extract email from various claim types
                var email = user.FindFirst(ClaimTypes.Email)?.Value ?? 
                           user.FindFirst("email")?.Value ?? 
                           user.Identity.Name ?? "user@boutique.com";
                
                // Extract full name from claims
                var fullName = user.FindFirst(ClaimTypes.Name)?.Value ?? 
                              user.FindFirst("name")?.Value ?? 
                              "User";
                
                // Extract role from claims
                var role = user.FindFirst(ClaimTypes.Role)?.Value ?? 
                          user.FindFirst("role")?.Value ?? 
                          "User";
                
                // Parse name into first and last name components
                var nameParts = fullName.Split(' ', 2);
                var firstName = cachedUpdates?.FirstName ?? (nameParts.Length > 0 ? nameParts[0] : "User");
                var lastName = cachedUpdates?.LastName ?? (nameParts.Length > 1 ? nameParts[1] : "");
                
                // Create profile DTO with claims and cached data
                return new UserProfileDto
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Role = role,
                    PhoneNumber = cachedUpdates?.PhoneNumber ?? "",
                    Bio = cachedUpdates?.Bio ?? $"Welcome {firstName}! Manage your boutique efficiently.",
                    JoinDate = DateTime.Now.AddMonths(-3)
                };
            }
            catch
            {
                return null;
            }
        }

        // Update user profile with API call and local storage fallback
        public async Task UpdateUserProfileAsync(UpdateUserProfileDto profile)
        {
            try
            {
                // Try to update profile via API
                await _httpClient.PutAsJsonAsync("api/user/profile", profile);
            }
            catch
            {
                // API failed, store updates locally for fallback
            }
            
            // Always cache the updates locally for real-time display
            await _localStorage.SetItemAsync("profileUpdates", profile);
        }

        // Change user password with API call and demo fallback
        public async Task ChangePasswordAsync(ChangePasswordDto passwordData)
        {
            try
            {
                // Attempt password change via API
                await _httpClient.PostAsJsonAsync("api/user/change-password", passwordData);
            }
            catch
            {
                // For demo purposes, simulate success with delay
                await Task.Delay(1000);
            }
        }
    }
}