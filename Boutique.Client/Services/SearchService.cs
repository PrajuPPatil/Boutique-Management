using Boutique.Client.Models;
using Boutique.Client.Models.DTOs;
using System.Net.Http.Json;

namespace Boutique.Client.Services
{
    // Global search service for searching across customers, orders, and payments
    public class SearchService
    {
        // HTTP client for API communication with search endpoints
        private readonly HttpClient _httpClient;

        // Constructor with dependency injection for HTTP client
        public SearchService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Perform global search across all entities (customers, orders, payments)
        public async Task<GlobalSearchResultDto> GlobalSearchAsync(string query)
        {
            var response = await _httpClient.GetAsync($"api/search/global?query={Uri.EscapeDataString(query)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GlobalSearchResultDto>() ?? new GlobalSearchResultDto();
        }
    }

    // Global search results containing matches from all entity types
    public class GlobalSearchResultDto
    {
        // Matching customers
        public List<CustomerDto> Customers { get; set; } = new();
        // Matching orders
        public List<OrderDto> Orders { get; set; } = new();
        // Matching payments
        public List<PaymentDto> Payments { get; set; } = new();
        // Total number of results across all categories
        public int TotalResults { get; set; }
    }
}