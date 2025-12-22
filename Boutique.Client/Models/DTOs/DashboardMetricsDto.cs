namespace Boutique.Client.Models.DTOs
{
    public class DashboardMetricsDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public decimal RevenueGrowth { get; set; }
        public decimal OrderGrowth { get; set; }
        public decimal CustomerGrowth { get; set; }
        public decimal CompletionRate { get; set; }
    }

    public class CustomerAnalyticsDto
    {
        public Dictionary<string, int> GenderDistribution { get; set; } = new();
        public List<TopCustomerDto> TopCustomers { get; set; } = new();
    }

    public class TopCustomerDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class PerformanceMetricsDto
    {
        public double AverageProcessingTime { get; set; }
        public decimal OnTimeDeliveryRate { get; set; }
        public decimal CustomerSatisfactionRate { get; set; }
        public decimal OrderCompletionRate { get; set; }
    }

    public class RevenueChartDto
    {
        public List<string> Labels { get; set; } = new();
        public List<decimal> Data { get; set; } = new();
    }
}