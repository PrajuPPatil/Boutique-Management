namespace WebApiBoutique.Services
{
    public interface IAnalyticsService
    {
        Task<DashboardAnalyticsDto> GetDashboardAnalyticsAsync();
        Task<RevenueChartDto> GetRevenueChartAsync(int months);
        Task<CustomerAnalyticsDto> GetCustomerAnalyticsAsync();
        Task<PerformanceMetricsDto> GetPerformanceMetricsAsync();
    }

    public class DashboardAnalyticsDto
    {
        public decimal TotalRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int MonthlyOrders { get; set; }
        public int TotalCustomers { get; set; }
        public int NewCustomers { get; set; }
        public decimal AverageOrderValue { get; set; }
        public double RevenueGrowth { get; set; }
        public double OrderGrowth { get; set; }
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