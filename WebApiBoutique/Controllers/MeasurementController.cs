using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApiBoutique.Models;
using WebApiBoutique.Services;
using WebApiBoutique.Models.DTOs;

namespace WebApiBoutique.Controllers
{
    // API controller for managing customer measurements
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MeasurementController : ControllerBase
    {
        // Dependency injection for measurement service
        private readonly IMeasurementService _measurementService;

        // Constructor to initialize measurement service
        public MeasurementController(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
        }
        
        private int GetBusinessIdFromToken()
        {
            var businessIdClaim = User.FindFirst("BusinessId")?.Value;
            return int.TryParse(businessIdClaim, out var businessId) ? businessId : 1;
        }

        // GET: api/Measurement/customer/{customerId} - Get all measurements for a customer
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<List<MeasurementDto>>> GetMeasurementsByCustomer(int customerId)
        {
            // Fetch all measurement records for a specific customer
            var measurements = await _measurementService.GetMeasurementsByCustomerAsync(customerId);
            return Ok(measurements);
        }

        // GET: api/Measurement/{id} - Get specific measurement by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<MeasurementDto>> GetMeasurementById(int id)
        {
            // Fetch measurement details by ID
            var measurement = await _measurementService.GetMeasurementByIdAsync(id);
            if (measurement == null)
                return NotFound(new { message = "Measurement not found" });
            return Ok(measurement);
        }

        // POST: api/Measurement - Create a new measurement record
        [HttpPost]
        public async Task<ActionResult> CreateMeasurement([FromBody] Measurement measurement)
        {
            // Set BusinessId from authenticated user
            measurement.BusinessId = GetBusinessIdFromToken();
            
            // Create new measurement with validation
            var created = await _measurementService.CreateMeasurementAsync(measurement);
            if (!created)
                return BadRequest(new { message = "Failed to create measurement" });
            return Ok(new { message = "Measurement created successfully" });
        }

        // PUT: api/Measurement/{id} - Update existing measurement
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMeasurement(int id, [FromBody] Measurement measurement)
        {
            // Update measurement values with validation
            var updated = await _measurementService.UpdateMeasurementAsync(id, measurement);
            if (!updated)
                return NotFound(new { message = "Measurement not found" });
            return Ok(new { message = "Measurement updated successfully" });
        }

        // DELETE: api/Measurement/{id} - Delete measurement record
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMeasurement(int id)
        {
            // Remove measurement record from database
            var deleted = await _measurementService.DeleteMeasurementAsync(id);
            if (!deleted)
                return NotFound(new { message = "Measurement not found" });
            return Ok(new { message = "Measurement deleted successfully" });
        }

        // POST: api/Measurement/validate - Validate measurement values against realistic ranges
        [HttpPost("validate")]
        public async Task<ActionResult<ValidationResult>> ValidateMeasurement([FromBody] ValidateMeasurementRequest request)
        {
            // Check if measurement values are within realistic ranges for gender/garment type
            var result = await _measurementService.ValidateMeasurementAsync(request.Gender, request.MeasurementType, request.Value);
            return Ok(result);
        }

        // GET: api/Measurement/statistics - Get measurement analytics and statistics
        [HttpGet("statistics")]
        public async Task<ActionResult<MeasurementStatisticsDto>> GetMeasurementStatistics()
        {
            // Get aggregated data: total measurements, popular garment types, etc.
            var statistics = await _measurementService.GetMeasurementStatisticsAsync();
            return Ok(statistics);
        }

        // GET: api/Measurement/recent - Get recently added measurements
        [HttpGet("recent")]
        public async Task<ActionResult<List<RecentMeasurementDto>>> GetRecentMeasurements([FromQuery] int limit = 10)
        {
            // Fetch latest measurement entries for dashboard display
            var recentMeasurements = await _measurementService.GetRecentMeasurementsAsync(limit);
            return Ok(recentMeasurements);
        }
    }

    // Data transfer object for measurement information
    public class MeasurementDto
    {
        public int MeasurementId { get; set; }  // Unique measurement identifier
        public string TypeName { get; set; } = "";  // Garment type (Shirt, Kurta, etc.)
        public string FabricColor { get; set; } = "";  // Selected fabric color
        public DateTime EntryDate { get; set; }  // When measurement was recorded
    }

    // Request model for measurement validation
    public class ValidateMeasurementRequest
    {
        public string Gender { get; set; } = string.Empty;  // Men/Women for different size ranges
        public string MeasurementType { get; set; } = string.Empty;  // Chest, Waist, etc.
        public decimal Value { get; set; }  // Measurement value to validate
    }
}