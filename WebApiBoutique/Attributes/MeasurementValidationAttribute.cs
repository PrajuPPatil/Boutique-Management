using System.ComponentModel.DataAnnotations;
using WebApiBoutique.Models;

namespace WebApiBoutique.Attributes
{
    // Custom validation attribute for customer measurements with realistic ranges
    public class MeasurementValidationAttribute : ValidationAttribute
    {
        // Static dictionary containing realistic measurement ranges by gender and type
        private static readonly Dictionary<string, Dictionary<string, (decimal min, decimal max)>> ValidationRanges = new()
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

        // Validate measurement value against realistic ranges for gender and type
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Check if value is a CustomerMeasurement object
            if (value is CustomerMeasurement measurement)
            {
                // Validate if gender and measurement type have defined ranges
                if (ValidationRanges.ContainsKey(measurement.Gender) &&
                    ValidationRanges[measurement.Gender].ContainsKey(measurement.MeasurementType))
                {
                    // Get min/max range for this gender and measurement type
                    var (min, max) = ValidationRanges[measurement.Gender][measurement.MeasurementType];
                    
                    // Check if measurement value is outside acceptable range
                    if (measurement.MeasurementValue < min || measurement.MeasurementValue > max)
                    {
                        // Create user-friendly error message
                        var genderText = measurement.Gender == "M" ? "men" : "women";
                        return new ValidationResult(
                            $"Invalid {measurement.MeasurementType.ToLower()} measurement for {genderText} - must be {min}-{max} inches");
                    }
                }
            }
            
            // Return success if validation passes
            return ValidationResult.Success;
        }
    }
}