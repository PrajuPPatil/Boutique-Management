using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiBoutique.Models
{
    // Entity model representing customer measurements for garment creation
    public class Measurement
    {
        // Primary key for measurement identification
        [Key]
        public int MeasurementId { get; set; }

        // Foreign key linking to the customer being measured
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }

        // Foreign key linking to garment type (Shirt, Kurta, etc.)
        [ForeignKey(nameof(Type))]
        public int TypeId { get; set; }

        // Path or URL to fabric image/pattern selected by customer
        [Required, StringLength(200)]
        public string FabricImage { get; set; } = string.Empty;

        // Color of fabric chosen for the garment
        [Required, StringLength(100)]
        public string FabricColor { get; set; } = string.Empty;

        // Date when measurements were taken
        [Required]
        public DateTime EntryDate { get; set; }
        
        // Multi-tenant support
        public int BusinessId { get; set; } = 1;

        // Navigation properties for related entities
        public virtual Customer Customer { get; set; } = null!;  // Customer being measured
        public virtual TypeModel Type { get; set; } = null!;     // Garment type details
    }
}
