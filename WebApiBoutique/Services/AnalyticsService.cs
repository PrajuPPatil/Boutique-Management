using Microsoft.EntityFrameworkCore;
using WebApiBoutique.Data;

namespace WebApiBoutique.Services
{
    // Service class for generating business analytics and performance metrics
    public class AnalyticsService : IAnalyticsService
    {
        // Database context for accessing business data
        private readonly AppDbContext _context;

        // Constructor to initialize database context
        public AnalyticsService(AppDbContext context)
        {
            _context = context;
        }

        // Generate comprehensive dashboard analytics with KPIs and growth metrics
        public async Task<DashboardAnalyticsDto> GetDashboardAnalyticsAsync()
        {
            // Set up date ranges for current and previous month comparisons
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var lastMonth = DateTime.Now.AddMonths(-1);

            // Calculate total revenue from all completed payments
            var totalRevenue = await _context.Payments
                .Where(p => p.Status == "Completed")  // Only count successful payments
                .SumAsync(p => p.Amount);

            // Calculate current month revenue for growth comparison
            var monthlyRevenue = await _context.Payments
                .Where(p => p.Status == "Completed" && 
                           p.PaymentDate.Month == currentMonth && 
                           p.PaymentDate.Year == currentYear)
                .SumAsync(p => p.Amount);

            // Calculate previous month revenue to determine growth rate
            var lastMonthRevenue = await _context.Payments
                .Where(p => p.Status == "Completed" && 
                           p.PaymentDate.Month == lastMonth.Month && 
                           p.PaymentDate.Year == lastMonth.Year)
                .SumAsync(p => p.Amount);

            var totalOrders = await _context.Orders.CountAsync();
            var monthlyOrders = await _context.Orders
                .Where(o => o.OrderDate.Month == currentMonth && o.OrderDate.Year == currentYear)
                .CountAsync();

            var lastMonthOrders = await _context.Orders
                .Where(o => o.OrderDate.Month == lastMonth.Month && o.OrderDate.Year == lastMonth.Year)
                .CountAsync();

            var totalCustomers = await _context.Customers.CountAsync();
            var newCustomers = await _context.Customers
                .Where(c => c.CreatedDate.Month == currentMonth && c.CreatedDate.Year == currentYear)
                .CountAsync();

            var lastMonthCustomers = await _context.Customers
                .Where(c => c.CreatedDate.Month == lastMonth.Month && c.CreatedDate.Year == lastMonth.Year)
                .CountAsync();

            // Calculate key performance indicators
            var averageOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0;

            // Calculate month-over-month growth percentages
            var revenueGrowth = lastMonthRevenue > 0 ? 
                (double)((monthlyRevenue - lastMonthRevenue) / lastMonthRevenue * 100) : 0;

            var orderGrowth = lastMonthOrders > 0 ? 
                (double)((monthlyOrders - lastMonthOrders) / (decimal)lastMonthOrders * 100) : 0;

            var customerGrowth = lastMonthCustomers > 0 ? 
                (double)((newCustomers - lastMonthCustomers) / (decimal)lastMonthCustomers * 100) : 0;

            return new DashboardAnalyticsDto
            {
                TotalRevenue = totalRevenue,
                MonthlyRevenue = monthlyRevenue,
                TotalOrders = totalOrders,
                MonthlyOrders = monthlyOrders,
                TotalCustomers = totalCustomers,
                NewCustomers = newCustomers,
                AverageOrderValue = averageOrderValue,
                RevenueGrowth = revenueGrowth,
                OrderGrowth = orderGrowth,
                CustomerGrowth = customerGrowth
            };
        }

        // Generate revenue trend data for chart visualization
        public async Task<RevenueChartDto> GetRevenueChartAsync(int months)
        {
            var startDate = DateTime.Now.AddMonths(-months);
            var labels = new List<string>();    // Month labels for chart
            var revenue = new List<decimal>();  // Revenue data points
            var orders = new List<int>();       // Order count data points

            // Generate data for each month in the specified range
            for (int i = months - 1; i >= 0; i--)
            {
                var date = DateTime.Now.AddMonths(-i);
                labels.Add(date.ToString("MMM yyyy"));  // Format: "Jan 2024"

                // Calculate revenue for this specific month
                var monthRevenue = await _context.Payments
                    .Where(p => p.Status == "Completed" && 
                               p.PaymentDate.Month == date.Month && 
                               p.PaymentDate.Year == date.Year)
                    .SumAsync(p => p.Amount);

                // Count orders for this specific month
                var monthOrders = await _context.Orders
                    .Where(o => o.OrderDate.Month == date.Month && o.OrderDate.Year == date.Year)
                    .CountAsync();

                revenue.Add(monthRevenue);
                orders.Add(monthOrders);
            }

            return new RevenueChartDto
            {
                Labels = labels,
                Revenue = revenue,
                Orders = orders
            };
        }

        // Generate customer demographics and behavior analytics
        public async Task<CustomerAnalyticsDto> GetCustomerAnalyticsAsync()
        {
            // Analyze customer distribution by gender
            var customersByGender = await _context.Customers
                .GroupBy(c => c.Gender)
                .ToDictionaryAsync(g => g.Key ?? "Unknown", g => g.Count());

            // Track customer acquisition trends over last 6 months
            var customersByMonth = new Dictionary<string, int>();
            for (int i = 5; i >= 0; i--)
            {
                var date = DateTime.Now.AddMonths(-i);
                var count = await _context.Customers
                    .Where(c => c.CreatedDate.Month == date.Month && c.CreatedDate.Year == date.Year)
                    .CountAsync();
                customersByMonth[date.ToString("MMM")] = count;  // Month abbreviation as key
            }

            // Identify top customers by total spending
            var topCustomers = await _context.Orders
                .Include(o => o.Customer)   // Load customer details
                .Include(o => o.Payments)   // Load payment information
                .GroupBy(o => new { o.CustomerId, o.Customer!.CustomerName })
                .Select(g => new TopCustomerDto
                {
                    CustomerName = g.Key.CustomerName,
                    TotalSpent = g.SelectMany(o => o.Payments).Sum(p => p.Amount),  // Sum all payments
                    OrderCount = g.Count()  // Count total orders
                })
                .OrderByDescending(c => c.TotalSpent)  // Sort by highest spenders
                .Take(5)  // Top 5 customers
                .ToListAsync();

            return new CustomerAnalyticsDto
            {
                CustomersByGender = customersByGender,
                CustomersByMonth = customersByMonth,
                TopCustomers = topCustomers
            };
        }

        // Calculate business performance metrics and operational KPIs
        public async Task<PerformanceMetricsDto> GetPerformanceMetricsAsync()
        {
            // Get all delivered orders for performance calculations
            var completedOrders = await _context.Orders
                .Where(o => o.Status == "Delivered")
                .ToListAsync();

            // Calculate average time from order to delivery
            var averageProcessingTime = completedOrders.Any() ? 
                completedOrders.Average(o => (DateTime.Now - o.OrderDate).TotalDays) : 0;

            // Calculate order completion rate percentage
            var totalOrders = await _context.Orders.CountAsync();
            var completedOrdersCount = completedOrders.Count;
            var orderCompletionRate = totalOrders > 0 ? (double)completedOrdersCount / totalOrders * 100 : 0;

            // Analyze order distribution by status
            var ordersByStatus = await _context.Orders
                .GroupBy(o => o.Status)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            // Analyze revenue distribution by payment method
            var revenueByPaymentMethod = await _context.Payments
                .Where(p => p.Status == "Completed")
                .GroupBy(p => p.PaymentMethod)
                .ToDictionaryAsync(g => g.Key, g => g.Sum(p => p.Amount));

            return new PerformanceMetricsDto
            {
                AverageOrderProcessingTime = averageProcessingTime,
                CustomerSatisfactionRate = 95.5, // Mock data - could be from surveys/feedback
                OrderCompletionRate = orderCompletionRate,
                OrdersByStatus = ordersByStatus,
                RevenueByPaymentMethod = revenueByPaymentMethod
            };
        }
    }
}