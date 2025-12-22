using WebApiBoutique.Models;
using WebApiBoutique.Services.Interface;

namespace WebApiBoutique.Services
{
    // Service for validating customer measurements against realistic ranges
    public class MeasurementValidationService : IMeasurementValidationService
    {
        // Dictionary containing realistic measurement ranges by gender and type (in inches)
        private readonly Dictionary<string, Dictionary<string, (decimal min, decimal max)>> _validationRanges;

        // Constructor initializes realistic measurement ranges for men and women
        public MeasurementValidationService()
        {
            // Define realistic measurement ranges based on standard sizing charts
            _validationRanges = new Dictionary<string, Dictionary<string, (decimal, decimal)>>
            {
                // Men's measurement ranges (M = Male)
                ["M"] = new Dictionary<string, (decimal, decimal)>
                {
                    ["Chest"] = (34, 52),        // Chest circumference in inches
                    ["Waist"] = (28, 44),        // Waist circumference in inches
                    ["Hips"] = (34, 48),         // Hip circumference in inches
                    ["Shoulder"] = (15, 22),     // Shoulder width in inches
                    ["Sleeve Length"] = (22, 27), // Sleeve length in inches
                    ["Neck"] = (14, 20)          // Neck circumference in inches
                },
                // Women's measurement ranges (F = Female)
                ["F"] = new Dictionary<string, (decimal, decimal)>
                {
                    ["Bust"] = (30, 46),         // Bust circumference in inches
                    ["Waist"] = (24, 40),        // Waist circumference in inches
                    ["Hips"] = (32, 48),         // Hip circumference in inches
                    ["Shoulder"] = (13, 19),     // Shoulder width in inches
                    ["Upper Arm"] = (10, 16)     // Upper arm circumference in inches
                }
            };
        }

        // Validate if measurement value falls within realistic range for gender/type
        public Task<bool> ValidateMeasurementAsync(string gender, string measurementType, decimal value)
        {
            // Check if gender and measurement type exist in validation ranges
            if (!_validationRanges.ContainsKey(gender) || 
                !_validationRanges[gender].ContainsKey(measurementType))
            {
                return Task.FromResult(false);
            }

            // Get min/max range and validate value is within bounds
            var (min, max) = _validationRanges[gender][measurementType];
            return Task.FromResult(value >= min && value <= max);
        }

        // Generate user-friendly error message for invalid measurements
        public Task<string> GetValidationErrorMessageAsync(string gender, string measurementType, decimal value)
        {
            // Handle unknown gender/measurement type combinations
            if (!_validationRanges.ContainsKey(gender) || 
                !_validationRanges[gender].ContainsKey(measurementType))
            {
                return Task.FromResult($"Invalid measurement type '{measurementType}' for gender '{gender}'");
            }

            // Create descriptive error message with valid range
            var (min, max) = _validationRanges[gender][measurementType];
            var genderText = gender == "M" ? "men" : "women";
            
            return Task.FromResult($"Invalid {measurementType.ToLower()} measurement for {genderText} - must be {min}-{max} inches");
        }

        // Get valid measurement range for specific gender and measurement type
        public Task<(decimal min, decimal max)> GetMeasurementRangeAsync(string gender, string measurementType)
        {
            // Return range if gender and measurement type exist
            if (_validationRanges.ContainsKey(gender) && 
                _validationRanges[gender].ContainsKey(measurementType))
            {
                return Task.FromResult(_validationRanges[gender][measurementType]);
            }

            // Return (0,0) for unknown combinations
            return Task.FromResult((0m, 0m));
        }

        // Validate measurement before insertion (throws exception if invalid)
        public async Task<bool> InsertValidatedMeasurementAsync(CustomerMeasurement measurement)
        {
            // Validate measurement against realistic ranges
            var isValid = await ValidateMeasurementAsync(
                measurement.Gender, 
                measurement.MeasurementType, 
                measurement.MeasurementValue);

            // Throw descriptive exception if validation fails
            if (!isValid)
            {
                var errorMessage = await GetValidationErrorMessageAsync(
                    measurement.Gender, 
                    measurement.MeasurementType, 
                    measurement.MeasurementValue);
                
                throw new ArgumentException(errorMessage);
            }

            return true;
        }
    }
}