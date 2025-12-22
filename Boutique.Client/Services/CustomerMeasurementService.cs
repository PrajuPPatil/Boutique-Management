using System.Net.Http.Json;
using Boutique.Client.Models;
using Boutique.Client.Models.DTOs;

namespace Boutique.Client.Services
{
    // Customer measurement service for handling individual measurement operations with validation
    public class CustomerMeasurementService
    {
        // HTTP client for API communication with customer measurement endpoints
        private readonly HttpClient _httpClient;

        // Constructor with dependency injection for HTTP client
        public CustomerMeasurementService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Create new customer measurement with validation
        public async Task<bool> CreateMeasurementAsync(CustomerMeasurement measurement)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/customermeasurement", measurement);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                // Return false on error for safe handling
                return false;
            }
        }

        // Validate measurement value against realistic ranges for gender and type
        public async Task<MeasurementValidationResponse> ValidateMeasurementAsync(string gender, string measurementType, decimal value)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<MeasurementValidationResponse>(
                    $"api/customermeasurement/validate?gender={gender}&measurementType={measurementType}&value={value}");
                return response ?? new MeasurementValidationResponse { IsValid = false, Message = "Validation failed" };
            }
            catch
            {
                // Return validation failure on error
                return new MeasurementValidationResponse { IsValid = false, Message = "Validation service unavailable" };
            }
        }

        // Get valid measurement ranges for specific gender (for frontend validation)
        public async Task<Dictionary<string, MeasurementRange>> GetMeasurementRangesAsync(string gender)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<Dictionary<string, MeasurementRange>>(
                    $"api/customermeasurement/ranges/{gender}");
                return response ?? new Dictionary<string, MeasurementRange>();
            }
            catch
            {
                // Return empty dictionary on error
                return new Dictionary<string, MeasurementRange>();
            }
        }

        public async Task<List<CustomerMeasurement>> GetCustomerMeasurementsAsync(int customerId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<CustomerMeasurement>>(
                    $"api/customermeasurement/customer/{customerId}");
                return response ?? new List<CustomerMeasurement>();
            }
            catch
            {
                return new List<CustomerMeasurement>();
            }
        }

        public async Task<List<CustomerMeasurement>> GetAllMeasurementsAsync()
        {
            try
            {
                var customers = await _httpClient.GetFromJsonAsync<List<CustomerDto>>("api/customer");
                var allMeasurements = new List<CustomerMeasurement>();
                
                if (customers != null)
                {
                    foreach (var customer in customers)
                    {
                        var measurements = await GetCustomerMeasurementsAsync(customer.CustomerId);
                        allMeasurements.AddRange(measurements);
                    }
                }
                
                return allMeasurements;
            }
            catch
            {
                return new List<CustomerMeasurement>();
            }
        }

        public async Task<CustomerMeasurement?> GetMeasurementAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<CustomerMeasurement>(
                    $"api/customermeasurement/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateMeasurementAsync(int id, CustomerMeasurement measurement)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/customermeasurement/{id}", measurement);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteMeasurementAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/customermeasurement/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }


    }
}