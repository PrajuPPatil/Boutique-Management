using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBoutique.Data;
using WebApiBoutique.Models;
using WebApiBoutique.Services.Interface;

namespace WebApiBoutique.Controllers
{
    // API controller for managing customer measurements with validation
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerMeasurementController : ControllerBase
    {
        // Database context for measurement operations
        private readonly AppDbContext _context;
        // Service for validating measurement values against realistic ranges
        private readonly IMeasurementValidationService _validationService;

        // Constructor with dependency injection for database and validation services
        public CustomerMeasurementController(AppDbContext context, IMeasurementValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }

        // Create new customer measurement with validation against realistic ranges
        [HttpPost]
        public async Task<IActionResult> CreateMeasurement([FromBody] CustomerMeasurement measurement)
        {
            try
            {
                // Log incoming measurement data for debugging
                Console.WriteLine($"Received: CustomerId={measurement.CustomerId}, Gender={measurement.Gender}, Type={measurement.MeasurementType}, Value={measurement.MeasurementValue}");
                
                // Validate model state (required fields, data types)
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    Console.WriteLine($"ModelState Invalid: {string.Join(", ", errors)}");
                    return BadRequest(new { error = "Validation failed", details = errors });
                }
                
                // Validate measurement value against realistic ranges for gender/type
                var hasRange = await _validationService.ValidateMeasurementAsync(
                    measurement.Gender, 
                    measurement.MeasurementType, 
                    measurement.MeasurementValue);
                
                // Check if validation failed for measurements with defined ranges
                if (!hasRange)
                {
                    var (min, max) = await _validationService.GetMeasurementRangeAsync(
                        measurement.Gender, 
                        measurement.MeasurementType);
                    
                    // If range exists (min/max not 0) but validation failed, return error
                    if (min > 0 || max > 0)
                    {
                        var errorMsg = await _validationService.GetValidationErrorMessageAsync(
                            measurement.Gender, 
                            measurement.MeasurementType, 
                            measurement.MeasurementValue);
                        Console.WriteLine($"Validation failed: {errorMsg}");
                        return BadRequest(new { error = errorMsg });
                    }
                }
                
                // Save measurement to database
                _context.CustomerMeasurements.Add(measurement);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Saved successfully with ID: {measurement.MeasurementId}");

                return Ok(new { message = "Measurement created successfully", measurementId = measurement.MeasurementId });
            }
            catch (Exception ex)
            {
                // Log and return server error
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Stack: {ex.StackTrace}");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        // Validate measurement value against realistic ranges (used by frontend)
        [HttpGet("validate")]
        public async Task<IActionResult> ValidateMeasurement(string gender, string measurementType, decimal value)
        {
            // Check if measurement value is within acceptable range
            var isValid = await _validationService.ValidateMeasurementAsync(gender, measurementType, value);
            
            // Return validation result with error message if invalid
            if (!isValid)
            {
                var errorMessage = await _validationService.GetValidationErrorMessageAsync(gender, measurementType, value);
                return Ok(new { isValid = false, message = errorMessage });
            }

            return Ok(new { isValid = true, message = "Measurement is valid" });
        }

        // Get valid measurement ranges for a specific gender (for frontend validation)
        [HttpGet("ranges/{gender}")]
        public async Task<IActionResult> GetMeasurementRanges(string gender)
        {
            var ranges = new Dictionary<string, object>();
            // Define measurement types based on gender (M=Men, F=Women)
            var measurementTypes = gender.ToUpper() == "M" 
                ? new[] { "Chest", "Waist", "Hips", "Shoulder", "Sleeve Length", "Neck" }
                : new[] { "Bust", "Waist", "Hips", "Shoulder", "Upper Arm" };

            // Get min/max ranges for each measurement type
            foreach (var type in measurementTypes)
            {
                var (min, max) = await _validationService.GetMeasurementRangeAsync(gender.ToUpper(), type);
                ranges[type] = new { min, max, unit = "inches" };
            }

            return Ok(ranges);
        }

        // Get all measurements for a specific customer (ordered by most recent)
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerMeasurements(int customerId)
        {
            try
            {
                Console.WriteLine($"Getting measurements for customer {customerId}");
                // Retrieve measurements ordered by creation date (newest first)
                var measurements = await _context.CustomerMeasurements
                    .Where(m => m.CustomerId == customerId)
                    .OrderByDescending(m => m.CreatedDate)
                    .ToListAsync();
                Console.WriteLine($"Found {measurements.Count} measurements");
                return Ok(measurements);
            }
            catch (Exception ex)
            {
                // Return empty list on error instead of throwing exception
                Console.WriteLine($"Error: {ex.Message}");
                return Ok(new List<CustomerMeasurement>());
            }
        }

        // Get single measurement by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMeasurement(int id)
        {
            // Find measurement by primary key
            var measurement = await _context.CustomerMeasurements
                .FirstOrDefaultAsync(m => m.MeasurementId == id);

            if (measurement == null)
                return NotFound(new { error = "Measurement not found" });

            return Ok(measurement);
        }

        // Update existing measurement with validation
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMeasurement(int id, [FromBody] CustomerMeasurement measurement)
        {
            try
            {
                // Find existing measurement
                var existing = await _context.CustomerMeasurements.FindAsync(id);
                if (existing == null)
                    return NotFound(new { error = "Measurement not found" });

                // Validate new measurement value against realistic ranges
                var hasRange = await _validationService.ValidateMeasurementAsync(
                    measurement.Gender, 
                    measurement.MeasurementType, 
                    measurement.MeasurementValue);
                
                // Check validation for measurements with defined ranges
                if (!hasRange)
                {
                    var (min, max) = await _validationService.GetMeasurementRangeAsync(
                        measurement.Gender, 
                        measurement.MeasurementType);
                    
                    if (min > 0 || max > 0)
                    {
                        var errorMsg = await _validationService.GetValidationErrorMessageAsync(
                            measurement.Gender, 
                            measurement.MeasurementType, 
                            measurement.MeasurementValue);
                        return BadRequest(new { error = errorMsg });
                    }
                }

                // Update measurement properties
                existing.Gender = measurement.Gender;
                existing.MeasurementType = measurement.MeasurementType;
                existing.MeasurementValue = measurement.MeasurementValue;
                existing.Unit = measurement.Unit;

                await _context.SaveChangesAsync();
                return Ok(new { message = "Measurement updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        // Delete single measurement by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeasurement(int id)
        {
            try
            {
                // Find measurement to delete
                var measurement = await _context.CustomerMeasurements.FindAsync(id);
                if (measurement == null)
                    return NotFound(new { error = "Measurement not found" });

                // Remove measurement from database
                _context.CustomerMeasurements.Remove(measurement);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Measurement deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        // Delete all measurements for a specific customer (bulk delete)
        [HttpDelete("customer/{customerId}")]
        public async Task<IActionResult> DeleteCustomerMeasurements(int customerId)
        {
            try
            {
                // Get all measurements for the customer
                var measurements = await _context.CustomerMeasurements
                    .Where(m => m.CustomerId == customerId)
                    .ToListAsync();

                // Remove all measurements in batch
                _context.CustomerMeasurements.RemoveRange(measurements);
                await _context.SaveChangesAsync();

                return Ok(new { message = $"Deleted {measurements.Count} measurements" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
    }
}