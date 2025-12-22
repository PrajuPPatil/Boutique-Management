using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiBoutique.Models
{
    // Entity representing individual customer measurements for garment creation
    public class CustomerMeasurement
    {
        // Primary key for measurement record
        [Key]
        public int MeasurementId { get; set; }

        // Foreign key linking to customer record
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }

        // Gender specification (M=Male, F=Female) for measurement validation
        public string Gender { get; set; } = string.Empty; // M or F
        
        // Type of garment being measured (Kurta, Shirt, Pant, Blazer, etc.)
        public string? GarmentType { get; set; } // Kurta, Shirt, Pant, etc.

        // Specific measurement type (Chest, Waist, Sleeve Length, etc.)
        public string MeasurementType { get; set; } = string.Empty;

        // Measurement value with precision for accurate tailoring
        [Column(TypeName = "decimal(5,2)")]
        public decimal MeasurementValue { get; set; }

        // Unit of measurement (default: inches)
        public string Unit { get; set; } = "inches";

        // Timestamp when measurement was recorded
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation property to customer entity
        public virtual Customer? Customer { get; set; }
    }
}