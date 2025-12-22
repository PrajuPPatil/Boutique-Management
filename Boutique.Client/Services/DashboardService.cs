using System.Net.Http.Json;

namespace Boutique.Client.Services
{
    // Dashboard service for retrieving dashboard statistics and recent activities
    public class DashboardService
    {
        // HTTP client for API communication with dashboard endpoints
        private readonly HttpClient _httpClient;

        // Constructor with dependency injection for HTTP client
        public DashboardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Get dashboard statistics for key performance indicators
        public async Task<DashboardStatsDto?> GetDashboardStatsAsync()
        {
            return await _httpClient.GetFromJsonAsync<DashboardStatsDto>("api/dashboard/stats");
        }

        // Get recent activities for dashboard activity feed
        public async Task<List<RecentActivityDto>?> GetRecentActivitiesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<RecentActivityDto>>("api/dashboard/recent-activities");
        }
    }

    // Dashboard statistics data for overview cards
    public class DashboardStatsDto
    {
        // Total number of customers
        public int TotalCustomers { get; set; }
        // Number of active orders
        public int ActiveOrders { get; set; }
        // Number of pending payments
        public int PendingPayments { get; set; }
        // Revenue for current month
        public decimal MonthlyRevenue { get; set; }
        // Total revenue across all time
        public decimal TotalRevenue { get; set; }
        // Number of completed orders
        public int CompletedOrders { get; set; }
    }

    // Recent activity data for dashboard activity feed
    public class RecentActivityDto
    {
        // Activity title for display
        public string Title { get; set; } = string.Empty;
        // Detailed activity description
        public string Description { get; set; } = string.Empty;
        // When activity occurred
        public DateTime Timestamp { get; set; }
        // Type of activity (customer, measurement, payment, order)
        public string ActivityType { get; set; } = string.Empty;
        // FontAwesome icon class for display
        public string Icon { get; set; } = string.Empty;
        // CSS color class for styling
        public string Color { get; set; } = string.Empty;
    }
}