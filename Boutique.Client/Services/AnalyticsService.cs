using Boutique.Client.Models.DTOs;
using System.Net.Http.Json;

namespace Boutique.Client.Services
{
    // Analytics service for business intelligence and reporting data
    public class AnalyticsService
    {
        // HTTP client for API communication with analytics endpoints
        private readonly HttpClient _httpClient;

        // Constructor with dependency injection for HTTP client
        public AnalyticsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Get dashboard analytics data for key performance indicators
        public async Task<DashboardAnalyticsDto> GetDashboardAnalyticsAsync()
        {
            var response = await _httpClient.GetAsync("api/analytics/dashboard");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<DashboardAnalyticsDto>() ?? new DashboardAnalyticsDto();
        }

        // Get revenue chart data for specified number of months
        public async Task<RevenueChartDto> GetRevenueChartAsync(int months = 6)
        {
            var response = await _httpClient.GetAsync($"api/analytics/revenue-chart?months={months}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<RevenueChartDto>() ?? new RevenueChartDto();
        }

        public async Task<CustomerAnalyticsDto> GetCustomerAnalyticsAsync()
        {
            var response = await _httpClient.GetAsync("api/analytics/customers");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CustomerAnalyticsDto>() ?? new CustomerAnalyticsDto();
        }

        public async Task<PerformanceMetricsDto> GetPerformanceMetricsAsync()
        {
            var response = await _httpClient.GetAsync("api/analytics/performance");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PerformanceMetricsDto>() ?? new PerformanceMetricsDto();
        }

        public async Task<DashboardAnalyticsDto> GetDashboardMetricsAsync()
        {
            return await GetDashboardAnalyticsAsync();
        }

        public async Task<Models.DTOs.RevenueChartDto> GetRevenueChartDataAsync()
        {
            var data = await GetRevenueChartAsync();
            return new Models.DTOs.RevenueChartDto
            {
                Labels = data.Labels,
                Data = data.Revenue
            };
        }
    }

    // Dashboard analytics data with key business metrics
    public class DashboardAnalyticsDto
    {
        // Total revenue across all time
        public decimal TotalRevenue { get; set; }
        // Revenue for current month
        public decimal MonthlyRevenue { get; set; }
        // Total orders placed
        public int TotalOrders { get; set; }
        // Orders placed this month
        public int MonthlyOrders { get; set; }
        // Total customer count
        public int TotalCustomers { get; set; }
        // New customers this month
        public int NewCustomers { get; set; }
        // Average value per order
        public decimal AverageOrderValue { get; set; }
        // Revenue growth percentage
        public double RevenueGrowth { get; set; }
        // Order growth percentage
        public double OrderGrowth { get; set; }
        // Customer growth percentage
        public double CustomerGrowth { get; set; }
    }

    public class RevenueChartDto
    {
        public List<string> Labels { get; set; } = new();
        public List<decimal> Revenue { get; set; } = new();
        public List<int> Orders { get; set; } = new();
    }

    public class CustomerAnalyticsDto
    {
        public Dictionary<string, int> CustomersByGender { get; set; } = new();
        public Dictionary<string, int> CustomersByMonth { get; set; } = new();
        public List<TopCustomerDto> TopCustomers { get; set; } = new();
    }

    public class TopCustomerDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalSpent { get; set; }
        public int OrderCount { get; set; }
    }

    public class PerformanceMetricsDto
    {
        public double AverageOrderProcessingTime { get; set; }
        public double CustomerSatisfactionRate { get; set; }
        public double OrderCompletionRate { get; set; }
        public Dictionary<string, int> OrdersByStatus { get; set; } = new();
        public Dictionary<string, decimal> RevenueByPaymentMethod { get; set; } = new();
    }
}