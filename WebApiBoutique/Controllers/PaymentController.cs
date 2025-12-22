using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiBoutique.Models.DTOs;
using WebApiBoutique.Services;

namespace WebApiBoutique.Controllers
{
    // API controller for managing payment operations
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        // Dependency injection for payment service
        private readonly IPaymentService _paymentService;

        // Constructor to initialize payment service
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET: api/Payment - Retrieve all payments for current business
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetAllPayments()
        {
            var businessId = int.Parse(User.FindFirst("BusinessId")?.Value ?? "0");
            var payments = await _paymentService.GetPaymentsByBusinessIdAsync(businessId);
            return Ok(payments);
        }

        // GET: api/Payment/{id} - Get specific payment by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDto>> GetPaymentById(int id)
        {
            // Fetch payment by ID from database
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
                return NotFound(new { message = "Payment not found" });
            return Ok(payment);
        }

        // GET: api/Payment/order/{orderId} - Get all payments for a specific order
        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPaymentsByOrder(int orderId)
        {
            // Fetch all payments associated with a specific order
            var payments = await _paymentService.GetPaymentsByOrderAsync(orderId);
            return Ok(payments);
        }

        // GET: api/Payment/customer/{customerId} - Get all payments for a specific customer
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPaymentsByCustomer(int customerId)
        {
            // Fetch payment history for a specific customer
            var payments = await _paymentService.GetPaymentsByCustomerAsync(customerId);
            return Ok(payments);
        }

        // GET: api/Payment/customer/{customerId}/summary - Get payment summary for customer
        [HttpGet("customer/{customerId}/summary")]
        public async Task<ActionResult<PaymentSummaryDto>> GetPaymentSummary(int customerId)
        {
            // Get aggregated payment data (total paid, pending, etc.) for customer
            var summary = await _paymentService.GetPaymentSummaryAsync(customerId);
            return Ok(summary);
        }

        // POST: api/Payment - Create a new payment record
        [HttpPost]
        public async Task<ActionResult<PaymentDto>> CreatePayment([FromBody] CreatePaymentDto createDto)
        {
            try
            {
                var businessId = int.Parse(User.FindFirst("BusinessId")?.Value ?? "0");
                var payment = await _paymentService.CreatePaymentAsync(createDto, businessId);
                return CreatedAtAction(nameof(GetPaymentById), new { id = payment.PaymentId }, payment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Payment/{id} - Update existing payment
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] UpdatePaymentDto updateDto)
        {
            // Update payment details (amount, method, status, etc.)
            var updated = await _paymentService.UpdatePaymentAsync(id, updateDto);
            if (!updated)
                return NotFound(new { message = "Payment not found" });
            return Ok(new { message = "Payment updated successfully" });
        }

        // DELETE: api/Payment/{id} - Delete payment record
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            // Remove payment record and adjust order balance
            var deleted = await _paymentService.DeletePaymentAsync(id);
            if (!deleted)
                return NotFound(new { message = "Payment not found" });
            return Ok(new { message = "Payment deleted successfully" });
        }
    }
}