using Microsoft.AspNetCore.Mvc;
using WebApiBoutique.Services;

namespace WebApiBoutique.Controllers
{
    // API controller for business analytics and reporting
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        // Dependency injection for analytics service
        private readonly IAnalyticsService _analyticsService;

        // Constructor to initialize analytics service
        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        // GET: api/Analytics/dashboard - Get main dashboard analytics data
        [HttpGet("dashboard")]
        public async Task<ActionResult<DashboardAnalyticsDto>> GetDashboardAnalytics()
        {
            // Fetch key metrics: total orders, revenue, customers, etc.
            var analytics = await _analyticsService.GetDashboardAnalyticsAsync();
            return Ok(analytics);
        }

        // GET: api/Analytics/revenue-chart - Get revenue data for chart visualization
        [HttpGet("revenue-chart")]
        public async Task<ActionResult<RevenueChartDto>> GetRevenueChart([FromQuery] int months = 6)
        {
            // Generate revenue trend data for specified number of months
            var chart = await _analyticsService.GetRevenueChartAsync(months);
            return Ok(chart);
        }

        // GET: api/Analytics/customers - Get customer-related analytics
        [HttpGet("customers")]
        public async Task<ActionResult<CustomerAnalyticsDto>> GetCustomerAnalytics()
        {
            // Analyze customer data: new customers, retention, demographics, etc.
            var analytics = await _analyticsService.GetCustomerAnalyticsAsync();
            return Ok(analytics);
        }

        // GET: api/Analytics/performance - Get business performance metrics
        [HttpGet("performance")]
        public async Task<ActionResult<PerformanceMetricsDto>> GetPerformanceMetrics()
        {
            // Calculate KPIs: order completion rate, average order value, delivery times, etc.
            var metrics = await _analyticsService.GetPerformanceMetricsAsync();
            return Ok(metrics);
        }
    }
}