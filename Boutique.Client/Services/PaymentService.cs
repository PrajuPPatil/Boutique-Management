using System.Net.Http.Json;
using Boutique.Client.Models.DTOs;
using System.Text.Json;

namespace Boutique.Client.Services
{
    // Payment management service for handling payment operations and financial transactions
    public class PaymentService
    {
        // HTTP client for API communication with payment endpoints
        private readonly HttpClient _httpClient;
        // JSON serialization options for case-insensitive property matching
        private readonly JsonSerializerOptions _jsonOptions;

        // Constructor with dependency injection for HTTP client
        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // Configure JSON options for API compatibility
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        // Get all payments from the system with error handling
        public async Task<List<PaymentDto>> GetPaymentsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<PaymentDto>>("api/payment", _jsonOptions) ?? new List<PaymentDto>();
            }
            catch
            {
                // Return empty list on error to prevent UI crashes
                return new List<PaymentDto>();
            }
        }

        // Get specific payment by ID with error handling
        public async Task<PaymentDto?> GetPaymentByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<PaymentDto>($"api/payment/{id}", _jsonOptions);
            }
            catch
            {
                // Return null on error for safe handling
                return null;
            }
        }

        // Get all payments for a specific customer
        public async Task<List<PaymentDto>> GetPaymentsByCustomerAsync(int customerId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<PaymentDto>>($"api/payment/customer/{customerId}", _jsonOptions) ?? new List<PaymentDto>();
            }
            catch
            {
                // Return empty list on error to prevent UI crashes
                return new List<PaymentDto>();
            }
        }

        // Add new payment record to the system
        public async Task<bool> AddPaymentAsync(CreatePaymentDto payment)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/payment", payment, _jsonOptions);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                // Return false on error for safe handling
                return false;
            }
        }

        // Update existing payment information
        public async Task<bool> UpdatePaymentAsync(int id, UpdatePaymentDto payment)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/payment/{id}", payment, _jsonOptions);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                // Return false on error for safe handling
                return false;
            }
        }

        // Delete payment record from the system
        public async Task<bool> DeletePaymentAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/payment/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                // Return false on error for safe handling
                return false;
            }
        }

        // Get payment summary for customer (total, paid, pending amounts)
        public async Task<PaymentSummaryDto?> GetPaymentSummaryAsync(int customerId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<PaymentSummaryDto>($"api/payment/customer/{customerId}/summary", _jsonOptions);
            }
            catch
            {
                // Return null on error for safe handling
                return null;
            }
        }
    }
}