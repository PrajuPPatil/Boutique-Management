using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBoutique.Models;
using WebApiBoutique.Models.DTOs;
using WebApiBoutique.ViewModels;
using WebApiBoutique.Data;

namespace WebApiBoutique.Controllers
{
    // API controller for managing delivery status tracking and customer delivery details
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryStatusController : ControllerBase
    {
        // Database context for delivery status operations
        private readonly AppDbContext _context;

        // Constructor with dependency injection for database context
        public DeliveryStatusController(AppDbContext context)
        {
            _context = context;
        }

        // Get all delivery statuses with associated customer information
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Delivery_Status>>> GetAll()
        {
            // Include customer details in delivery status query
            var deliveries = await _context.DeliveryStatuses
                .Include(d => d.Customer)
                .ToListAsync();

            return Ok(deliveries);
        }

        // Get single delivery status by ID with customer details
        [HttpGet("{id}")]
        public async Task<ActionResult<Delivery_Status>> GetById(int id)
        {
            // Find delivery status with customer information
            var delivery = await _context.DeliveryStatuses
                .Include(d => d.Customer)
                .FirstOrDefaultAsync(d => d.DeliveryId == id);

            if (delivery == null)
                return NotFound();

            return Ok(delivery);
        }

        // Get delivery status by customer name using stored procedure for complex queries
        [HttpGet("by-customer")]
        public async Task<IActionResult> GetByCustomerName([FromQuery] string name)
        {
            // Execute stored procedure to get customer delivery details by name
            var results = await _context.Set<CustomerDeliveryDTO>()
                .FromSqlInterpolated($"EXEC GetCustomerDeliveryDetailsByName {name}")
                .ToListAsync();

            if (results == null || !results.Any())
                return NotFound("No delivery records found for this customer.");

            // Map DTOs to view models for frontend consumption
            var viewModels = results.Select(dto => new CustomerDeliveryDetailsViewModel
            {
                CustomerId = dto.CustomerId,
                CustomerName = dto.CustomerName,
                Email = dto.Email,
                PhoneNo = dto.PhoneNo,
                DeliveryId = dto.DeliveryId,
                Status = dto.Status,
                EntryDate = dto.EntryDate,
                AdvanceAmount = dto.AdvanceAmount,
                AdvanceDate = dto.AdvanceDate,
                PaymentStatus = dto.PaymentStatus
            }).ToList();

            return Ok(viewModels);
        }

        // Create new delivery status record
        [HttpPost]
        public async Task<ActionResult<Delivery_Status>> Create(Delivery_Status delivery)
        {
            // Add new delivery status to database
            _context.DeliveryStatuses.Add(delivery);
            await _context.SaveChangesAsync();

            // Return created resource with location header
            return CreatedAtAction(nameof(GetById), new { id = delivery.DeliveryId }, delivery);
        }

        // Update existing delivery status
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Delivery_Status delivery)
        {
            // Validate that route ID matches entity ID
            if (id != delivery.DeliveryId)
                return BadRequest();

            // Mark entity as modified for EF Core tracking
            _context.Entry(delivery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrent update conflicts
                if (!await _context.DeliveryStatuses.AnyAsync(d => d.DeliveryId == id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        // Delete delivery status record
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Find delivery status to delete
            var delivery = await _context.DeliveryStatuses.FindAsync(id);
            if (delivery == null)
                return NotFound();

            // Remove delivery status from database
            _context.DeliveryStatuses.Remove(delivery);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
