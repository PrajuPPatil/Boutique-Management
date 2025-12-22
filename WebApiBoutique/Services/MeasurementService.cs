using Microsoft.EntityFrameworkCore;
using WebApiBoutique.Data;
using WebApiBoutique.Models;
using WebApiBoutique.Models.DTOs;

namespace WebApiBoutique.Services
{
    // Service class implementing measurement business logic and validation
    public class MeasurementService : IMeasurementService
    {
        // Database context for measurement data access
        private readonly AppDbContext _context;

        // Constructor to initialize database context
        public MeasurementService(AppDbContext context)
        {
            _context = context;
        }

        // Get all measurements for a specific customer with garment type information
        public async Task<List<MeasurementDto>> GetMeasurementsByCustomerAsync(int customerId)
        {
            // Join measurements with garment types to get complete information
            return await _context.Measurements
                .Where(m => m.CustomerId == customerId)  // Filter by customer
                .Join(_context.Types, m => m.TypeId, t => t.TypeId, (m, t) => new MeasurementDto  // Join with Types table
                {
                    MeasurementId = m.MeasurementId,
                    TypeName = t.TypeName,
                    FabricColor = m.FabricColor,
                    EntryDate = m.EntryDate
                })
                .OrderByDescending(m => m.EntryDate)  // Show most recent measurements first
                .ToListAsync();
        }

        public async Task<MeasurementDto?> GetMeasurementByIdAsync(int id)
        {
            return await _context.Measurements
                .Where(m => m.MeasurementId == id)
                .Join(_context.Types, m => m.TypeId, t => t.TypeId, (m, t) => new MeasurementDto
                {
                    MeasurementId = m.MeasurementId,
                    TypeName = t.TypeName,
                    FabricColor = m.FabricColor,
                    EntryDate = m.EntryDate
                })
                .FirstOrDefaultAsync();
        }

        // Create new measurement record with error handling
        public async Task<bool> CreateMeasurementAsync(Measurement measurement)
        {
            try
            {
                // Add measurement to database
                _context.Measurements.Add(measurement);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                // Return false if creation fails (validation errors, etc.)
                return false;
            }
        }

        // Update existing measurement with new values
        public async Task<bool> UpdateMeasurementAsync(int id, Measurement measurement)
        {
            // Find existing measurement
            var existing = await _context.Measurements.FindAsync(id);
            if (existing == null) return false;

            // Update measurement fields
            existing.TypeId = measurement.TypeId;
            existing.FabricColor = measurement.FabricColor;
            existing.EntryDate = measurement.EntryDate;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMeasurementAsync(int id)
        {
            var measurement = await _context.Measurements.FindAsync(id);
            if (measurement == null) return false;

            _context.Measurements.Remove(measurement);
            await _context.SaveChangesAsync();
            return true;
        }

        // Validate measurement values against realistic ranges for gender and type
        public Task<ValidationResult> ValidateMeasurementAsync(string gender, string measurementType, decimal value)
        {
            var result = new ValidationResult { IsValid = true };

            // Get appropriate ranges based on gender
            var ranges = gender == "M" ? GetMenRanges() : GetWomenRanges();

            // Check if value is within acceptable range
            if (ranges.TryGetValue(measurementType, out var range))
            {
                if (value < range.Min || value > range.Max)
                {
                    result.IsValid = false;
                    result.Message = $"{measurementType} must be between {range.Min} and {range.Max} inches";
                }
            }

            return Task.FromResult(result);
        }

        // Define realistic measurement ranges for men's garments (in inches)
        private Dictionary<string, (decimal Min, decimal Max)> GetMenRanges()
        {
            return new Dictionary<string, (decimal, decimal)>
            {
                { "Chest", (34, 52) },        // Men's chest: 34-52 inches
                { "Waist", (28, 44) },        // Men's waist: 28-44 inches
                { "Hips", (34, 48) },         // Men's hips: 34-48 inches
                { "Shoulder", (15, 22) },     // Shoulder width: 15-22 inches
                { "Sleeve Length", (22, 27) }, // Sleeve length: 22-27 inches
                { "Neck", (14, 20) }          // Neck size: 14-20 inches
            };
        }

        // Define realistic measurement ranges for women's garments (in inches)
        private Dictionary<string, (decimal Min, decimal Max)> GetWomenRanges()
        {
            return new Dictionary<string, (decimal, decimal)>
            {
                { "Bust", (30, 46) },         // Women's bust: 30-46 inches
                { "Waist", (24, 40) },        // Women's waist: 24-40 inches
                { "Hips", (32, 48) },         // Women's hips: 32-48 inches
                { "Shoulder", (13, 19) },     // Shoulder width: 13-19 inches
                { "Upper Arm", (10, 16) }     // Upper arm: 10-16 inches
            };
        }

        // Generate comprehensive measurement statistics for analytics dashboard
        public async Task<MeasurementStatisticsDto> GetMeasurementStatisticsAsync()
        {
            var today = DateTime.Today;
            
            // Count unique customer-garment combinations (one measurement per customer per garment type)
            var totalMeasurements = await _context.CustomerMeasurements
                .GroupBy(m => new { m.CustomerId, m.GarmentType })
                .CountAsync();
            
            var totalCustomers = await _context.CustomerMeasurements
                .Select(m => m.CustomerId)
                .Distinct()
                .CountAsync();
            
            var menMeasurements = await _context.CustomerMeasurements
                .Where(m => m.Gender == "M")
                .GroupBy(m => new { m.CustomerId, m.GarmentType })
                .CountAsync();
            
            var womenMeasurements = await _context.CustomerMeasurements
                .Where(m => m.Gender == "F")
                .GroupBy(m => new { m.CustomerId, m.GarmentType })
                .CountAsync();
            
            var todayMeasurements = await _context.CustomerMeasurements
                .Where(m => m.CreatedDate.Date == today)
                .GroupBy(m => new { m.CustomerId, m.GarmentType })
                .CountAsync();
            
            var garmentTypeCounts = await _context.CustomerMeasurements
                .Where(m => !string.IsNullOrEmpty(m.GarmentType))
                .GroupBy(m => m.GarmentType)
                .Select(g => new { GarmentType = g.Key, Count = g.GroupBy(x => x.CustomerId).Count() })
                .ToDictionaryAsync(x => x.GarmentType, x => x.Count);

            return new MeasurementStatisticsDto
            {
                TotalMeasurements = totalMeasurements,
                TotalCustomers = totalCustomers,
                MenMeasurements = menMeasurements,
                WomenMeasurements = womenMeasurements,
                TodayMeasurements = todayMeasurements,
                GarmentTypeCounts = garmentTypeCounts
            };
        }

        // Get recently added measurements for dashboard display
        public async Task<List<RecentMeasurementDto>> GetRecentMeasurementsAsync(int limit = 10)
        {
            // Fetch latest measurements with customer information
            return await _context.CustomerMeasurements
                .Where(m => !string.IsNullOrEmpty(m.GarmentType))  // Filter out invalid entries
                .Include(m => m.Customer)  // Load customer details
                .OrderByDescending(m => m.CreatedDate)  // Sort by most recent
                .GroupBy(m => new { m.CustomerId, m.GarmentType, m.Customer.CustomerName })  // Group by customer and garment
                .Select(g => new RecentMeasurementDto
                {
                    MeasurementId = g.First().MeasurementId,
                    CustomerName = g.Key.CustomerName,
                    Gender = g.First().Gender == "M" ? "Men" : "Women",
                    GarmentType = g.Key.GarmentType,
                    CreatedDate = g.Max(m => m.CreatedDate),
                    CustomerId = g.Key.CustomerId
                })
                .OrderByDescending(m => m.CreatedDate)
                .Take(limit)
                .ToListAsync();
        }
    }
}