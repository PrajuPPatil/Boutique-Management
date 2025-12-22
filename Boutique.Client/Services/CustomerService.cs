using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Boutique.Client.Models.DTOs;

namespace Boutique.Client.Services
{
    // Customer management service for handling customer CRUD operations
    public class CustomerService
    {
        // HTTP client for API communication with customer endpoints
        private readonly HttpClient _httpClient;

        // Constructor with dependency injection for HTTP client
        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Get all customers from the API
        public async Task<List<CustomerDto>?> GetCustomersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CustomerDto>>("api/customer");
        }

        // Get specific customer by ID
        public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<CustomerDto>($"api/customer/{id}");
        }

        // Add new customer to the system
        public async Task<CustomerDto> AddCustomerAsync(CustomerDto customer)
        {
            // Send the complete customer object to API
            var response = await _httpClient.PostAsJsonAsync("api/customer", customer);
            
            // Handle creation errors with detailed error messages
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Customer creation failed: {response.StatusCode} - {errorContent}");
            }
            
            // Return created customer data
            return await response.Content.ReadFromJsonAsync<CustomerDto>() ?? customer;
        }

        // Update existing customer information
        public async Task UpdateCustomerAsync(int id, CustomerDto customer)
        {
            await _httpClient.PutAsJsonAsync($"api/customer/{id}", customer);
        }

        // Delete customer from the system
        public async Task DeleteCustomerAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/customer/{id}");
        }

        // Check for duplicate email or phone number before customer creation
        public async Task<DuplicateCheckResult> CheckDuplicateAsync(string? email = null, string? phone = null)
        {
            // Build query parameters for duplicate check
            var query = new List<string>();
            if (!string.IsNullOrEmpty(email)) query.Add($"email={Uri.EscapeDataString(email)}");
            if (!string.IsNullOrEmpty(phone)) query.Add($"phone={Uri.EscapeDataString(phone)}");
            
            // Create query string and send request
            var queryString = string.Join("&", query);
            var response = await _httpClient.GetFromJsonAsync<DuplicateCheckResult>($"api/customer/check-duplicate?{queryString}");
            return response ?? new DuplicateCheckResult();
        }
    }

    // Result model for duplicate check operations
    public class DuplicateCheckResult
    {
        // Indicates if email already exists in system
        public bool EmailExists { get; set; }
        // Indicates if phone number already exists in system
        public bool PhoneExists { get; set; }
    }
}
