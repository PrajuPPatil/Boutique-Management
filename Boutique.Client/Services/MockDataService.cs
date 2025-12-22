namespace Boutique.Client.Services
{
    public static class MockDataService
    {
        public static DashboardStatsDto GetMockDashboardStats()
        {
            return new DashboardStatsDto
            {
                TotalCustomers = 156,
                ActiveOrders = 23,
                PendingPayments = 8,
                MonthlyRevenue = 45000,
                TotalRevenue = 250000,
                CompletedOrders = 89
            };
        }

        public static List<RecentActivityDto> GetMockRecentActivities()
        {
            return new List<RecentActivityDto>
            {
                new RecentActivityDto
                {
                    Title = "New customer registered",
                    Description = "Priya Sharma joined as a new customer",
                    Timestamp = DateTime.Now.AddHours(-2),
                    ActivityType = "customer",
                    Icon = "fas fa-user-plus",
                    Color = "bg-primary"
                },
                new RecentActivityDto
                {
                    Title = "Measurement completed",
                    Description = "Measurements recorded for Kurti order",
                    Timestamp = DateTime.Now.AddHours(-4),
                    ActivityType = "measurement",
                    Icon = "fas fa-ruler",
                    Color = "bg-success"
                },
                new RecentActivityDto
                {
                    Title = "Payment received",
                    Description = "₹2,500 payment received from Anjali Patel",
                    Timestamp = DateTime.Now.AddHours(-6),
                    ActivityType = "payment",
                    Icon = "fas fa-credit-card",
                    Color = "bg-warning"
                },
                new RecentActivityDto
                {
                    Title = "Order delivered",
                    Description = "Saree order delivered to Meera Singh",
                    Timestamp = DateTime.Now.AddDays(-1),
                    ActivityType = "order",
                    Icon = "fas fa-truck",
                    Color = "bg-info"
                },
                new RecentActivityDto
                {
                    Title = "New order placed",
                    Description = "Lehenga order placed by Kavya Reddy",
                    Timestamp = DateTime.Now.AddDays(-2),
                    ActivityType = "order",
                    Icon = "fas fa-shopping-bag",
                    Color = "bg-success"
                }
            };
        }
    }
}