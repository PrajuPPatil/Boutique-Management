using System.ComponentModel.DataAnnotations;

namespace Boutique.Client.Models
{
    public class CustomerMeasurement
    {
        public int MeasurementId { get; set; }
        public int CustomerId { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string? GarmentType { get; set; }
        public string MeasurementType { get; set; } = string.Empty;
        public decimal MeasurementValue { get; set; }
        public string Unit { get; set; } = "inches";
        public DateTime CreatedDate { get; set; }
    }

    public class MeasurementValidationResponse
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class MeasurementRange
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public string Unit { get; set; } = "inches";
    }
}