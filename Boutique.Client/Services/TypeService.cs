// Client-side service for managing garment types through API communication
// Handles all HTTP requests to the Type controller endpoints
using System.Net.Http.Json;
using Boutique.Client.Models;

namespace Boutique.Client.Services
{
    // Service class for garment type management operations
    public class TypeService
    {
        // HTTP client for making API requests to backend
        private readonly HttpClient _httpClient;

        // Constructor to initialize HTTP client with dependency injection
        public TypeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Retrieve all garment types from the backend API
        // Returns: List of TypeModel objects or empty list if request fails
        public async Task<List<TypeModel>> GetTypesAsync()
        {
            // GET request to /api/Type endpoint, deserialize JSON response to List<TypeModel>
            return await _httpClient.GetFromJsonAsync<List<TypeModel>>("api/Type") ?? new();
        }

        // Retrieve a specific garment type by its ID
        // Parameters: id - The unique identifier of the garment type
        // Returns: TypeModel object or null if not found
        public async Task<TypeModel?> GetTypeAsync(int id)
        {
            // GET request to /api/Type/{id} endpoint
            return await _httpClient.GetFromJsonAsync<TypeModel>($"api/Type/{id}");
        }

        // Create a new garment type in the system
        // Parameters: type - TypeModel object containing the new type data
        // Returns: true if creation successful, false otherwise
        public async Task<bool> CreateTypeAsync(TypeModel type)
        {
            // POST request to /api/Type endpoint with JSON payload
            var response = await _httpClient.PostAsJsonAsync("api/Type", type);
            // Check if HTTP status code indicates success (200-299 range)
            return response.IsSuccessStatusCode;
        }

        // Update an existing garment type
        // Parameters: type - TypeModel object with updated data (must include TypeId)
        // Returns: true if update successful, false otherwise
        public async Task<bool> UpdateTypeAsync(TypeModel type)
        {
            // PUT request to /api/Type/{id} endpoint with JSON payload
            var response = await _httpClient.PutAsJsonAsync($"api/Type/{type.TypeId}", type);
            return response.IsSuccessStatusCode;
        }

        // Delete a garment type from the system
        // Parameters: id - The unique identifier of the type to delete
        // Returns: true if deletion successful, false otherwise
        public async Task<bool> DeleteTypeAsync(int id)
        {
            // DELETE request to /api/Type/{id} endpoint
            var response = await _httpClient.DeleteAsync($"api/Type/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}