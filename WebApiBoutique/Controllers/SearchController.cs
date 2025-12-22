using Microsoft.AspNetCore.Mvc;
using WebApiBoutique.Services;
using WebApiBoutique.Models.DTOs;

namespace WebApiBoutique.Controllers
{
    // API controller for global search functionality across all entities
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        // Dependency injection for multiple services to search across entities
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;

        // Constructor to initialize all required services for comprehensive search
        public SearchController(ICustomerService customerService, IOrderService orderService, IPaymentService paymentService)
        {
            _customerService = customerService;
            _orderService = orderService;
            _paymentService = paymentService;
        }

        // GET: api/Search/global - Perform global search across customers, orders, and payments
        [HttpGet("global")]
        public async Task<ActionResult<GlobalSearchResultDto>> GlobalSearch([FromQuery] string query)
        {
            // Validate search query input
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Search query cannot be empty");

            var result = new GlobalSearchResultDto();

            // Search customers by name, email, or phone number
            var customers = await _customerService.GetCustomersByBusinessIdAsync(1); // Use default business ID for now
            result.Customers = customers.Select(c => new CustomerDto
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                Email = c.Email,
                PhoneNo = c.PhoneNo,
                Address = c.Address,
                Gender = c.Gender
            }).Where(c => 
                // Case-insensitive search across customer fields
                c.CustomerName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                c.Email.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                c.PhoneNo.Contains(query, StringComparison.OrdinalIgnoreCase)
            ).Take(10).ToList();  // Limit to 10 results for performance

            // Search orders by customer name, order ID, or garment type
            var orders = await _orderService.GetAllOrdersAsync();
            result.Orders = orders.Where(o => 
                o.CustomerName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                o.OrderId.ToString().Contains(query) ||  // Search by order ID
                o.TypeName.Contains(query, StringComparison.OrdinalIgnoreCase)  // Search by garment type
            ).Take(10).ToList();

            // Search payments by customer name, payment ID, or payment method
            var payments = await _paymentService.GetAllPaymentsAsync();
            result.Payments = payments.Where(p => 
                p.CustomerName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                p.PaymentId.ToString().Contains(query) ||  // Search by payment ID
                p.PaymentMethod.Contains(query, StringComparison.OrdinalIgnoreCase)  // Search by method (Cash, Card, etc.)
            ).Take(10).ToList();

            // Calculate total results across all categories
            result.TotalResults = result.Customers.Count + result.Orders.Count + result.Payments.Count;

            return Ok(result);
        }
    }

    // Data transfer object for global search results
    public class GlobalSearchResultDto
    {
        public List<CustomerDto> Customers { get; set; } = new();  // Matching customers
        public List<OrderDto> Orders { get; set; } = new();  // Matching orders
        public List<PaymentDto> Payments { get; set; } = new();  // Matching payments
        public int TotalResults { get; set; }  // Total count across all categories
    }
}