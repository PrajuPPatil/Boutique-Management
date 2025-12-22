using System.Text.Json;
using Boutique.Client.Models.DTOs;

namespace Boutique.Client.Services
{
    // Measurement service for handling customer measurements and statistics
    public class MeasurementService
    {
        // HTTP client for API communication with measurement endpoints
        private readonly HttpClient _httpClient;
        // JSON serialization options for case-insensitive property matching
        private readonly JsonSerializerOptions _jsonOptions;

        // Constructor with dependency injection for HTTP client
        public MeasurementService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // Configure JSON options for API compatibility
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        // Get all measurements for a specific customer
        public async Task<List<MeasurementDto>> GetMeasurementsByCustomerAsync(int customerId)
        {
            var response = await _httpClient.GetAsync($"api/measurement/customer/{customerId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<MeasurementDto>>(json, _jsonOptions) ?? new List<MeasurementDto>();
            }
            return new List<MeasurementDto>();
        }

        public async Task<PaymentSummaryDto?> GetPaymentSummaryAsync(int customerId)
        {
            var response = await _httpClient.GetAsync($"api/payment/customer/{customerId}/summary");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<PaymentSummaryDto>(json, _jsonOptions);
            }
            return null;
        }

        // Get measurement statistics for dashboard analytics
        public async Task<MeasurementStatisticsDto?> GetMeasurementStatisticsAsync()
        {
            var response = await _httpClient.GetAsync("api/measurement/statistics");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<MeasurementStatisticsDto>(json, _jsonOptions);
            }
            return null;
        }

        // Get recent measurements with optional limit for dashboard display
        public async Task<List<RecentMeasurementDto>> GetRecentMeasurementsAsync(int limit = 10)
        {
            var response = await _httpClient.GetAsync($"api/measurement/recent?limit={limit}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<RecentMeasurementDto>>(json, _jsonOptions) ?? new List<RecentMeasurementDto>();
            }
            return new List<RecentMeasurementDto>();
        }
    }

    // Payment summary data for customer financial overview
    public class PaymentSummaryDto
    {
        // Total amount across all orders
        public decimal TotalAmount { get; set; }
        // Amount already paid by customer
        public decimal PaidAmount { get; set; }
        // Outstanding balance remaining
        public decimal PendingBalance { get; set; }
        // Total number of orders for customer
        public int TotalOrders { get; set; }
    }
}