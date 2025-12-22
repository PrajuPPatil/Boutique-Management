using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiBoutique.Models.DTOs;
using WebApiBoutique.Services;

namespace WebApiBoutique.Controllers
{
    // API controller for managing order operations
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        // Dependency injection for order service
        private readonly IOrderService _orderService;

        // Constructor to initialize order service
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/Order - Retrieve all orders for current business
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            try
            {
                var businessId = int.Parse(User.FindFirst("BusinessId")?.Value ?? "0");
                var orders = await _orderService.GetOrdersByBusinessIdAsync(businessId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Orders table not found. Please run the SQL script to create it.", error = ex.Message });
            }
        }

        // GET: api/Order/active - Get only active orders for current business
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetActiveOrders()
        {
            var businessId = int.Parse(User.FindFirst("BusinessId")?.Value ?? "0");
            var orders = await _orderService.GetActiveOrdersByBusinessIdAsync(businessId);
            return Ok(orders);
        }

        // GET: api/Order/{id} - Get specific order by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            // Fetch order by ID from database
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        // GET: api/Order/customer/{customerId} - Get all orders for a specific customer
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByCustomer(int customerId)
        {
            // Fetch all orders belonging to a specific customer
            var orders = await _orderService.GetOrdersByCustomerAsync(customerId);
            return Ok(orders);
        }

        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetFilteredOrders([FromBody] OrderFilterDto filter)
        {
            var businessId = int.Parse(User.FindFirst("BusinessId")?.Value ?? "0");
            var orders = await _orderService.GetFilteredOrdersAsync(filter, businessId);
            return Ok(orders);
        }

        // POST: api/Order - Create a new order
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            Console.WriteLine($"CreateOrder called with CustomerId: {createOrderDto.CustomerId}, MeasurementId: {createOrderDto.MeasurementId}");
            try
            {
                var businessId = int.Parse(User.FindFirst("BusinessId")?.Value ?? "0");
                var order = await _orderService.CreateOrderAsync(createOrderDto, businessId);
                Console.WriteLine($"Order created successfully with ID: {order.OrderId}");
                return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating order: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return BadRequest(new { message = ex.Message, details = ex.InnerException?.Message });
            }
        }

        // PUT: api/Order/{id}/status - Update order status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto updateDto)
        {
            // Update order status (Pending, InProgress, ReadyForDelivery, Delivered)
            var updated = await _orderService.UpdateOrderStatusAsync(id, updateDto);
            if (!updated)
                return NotFound();
            return Ok(new { message = "Order status updated successfully" });
        }

        // GET: api/Order/paginated - Get orders with pagination and filtering
        [HttpGet("paginated")]
        public async Task<ActionResult<PaginatedOrdersDto>> GetPaginatedOrders(
            [FromQuery] int page = 1,  // Page number (default: 1)
            [FromQuery] int pageSize = 10,  // Items per page (default: 10)
            [FromQuery] string? status = null,  // Filter by order status
            [FromQuery] string? priority = null)  // Filter by priority level
        {
            var businessId = int.Parse(User.FindFirst("BusinessId")?.Value ?? "0");
            var result = await _orderService.GetPaginatedOrdersAsync(page, pageSize, status, priority, businessId);
            return Ok(result);
        }

        [HttpGet("{id}/timeline")]
        public async Task<ActionResult<OrderTimelineDto>> GetOrderTimeline(int id)
        {
            var timeline = await _orderService.GetOrderTimelineAsync(id);
            if (timeline == null)
                return NotFound();
            return Ok(timeline);
        }

        // DELETE: api/Order/{id} - Soft delete order (deactivate)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            // Soft delete order (mark as inactive instead of permanent deletion)
            var deleted = await _orderService.DeleteOrderAsync(id);
            if (!deleted)
                return NotFound();
            return Ok(new { message = "Order deactivated successfully" });
        }
    }
}