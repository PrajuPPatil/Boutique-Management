using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiBoutique.Models
{
    // Entity for men's kurta measurements with traditional Indian garment specifications
    public class M_KurtaMen
    {
        // Primary key for kurta measurement record
        [Key]
        public int KurtaMenId { get; set; }

        // Foreign key linking to base measurement record
        [ForeignKey(nameof(Measurement))]
        public int MeasurementId { get; set; }

        // Foreign key linking to garment type definition
        [ForeignKey(nameof(Type))]
        public int TypeId { get; set; }

        // Chest circumference measurement (inches)
        [Required, Range(0, 200)]
        public decimal Chest { get; set; }

        // Waist circumference measurement (inches)
        [Required, Range(0, 200)]
        public decimal Waist { get; set; }

        // Hip circumference measurement (inches)
        [Required, Range(0, 200)]
        public decimal Hip { get; set; }

        // Shoulder width measurement (inches)
        [Required, Range(0, 100)]
        public decimal ShoulderWidth { get; set; }

        // Sleeve length from shoulder to wrist (inches)
        [Required, Range(0, 100)]
        public decimal SleeveLength { get; set; }

        // Armhole circumference measurement (inches)
        [Required, Range(0, 100)]
        public decimal Armhole { get; set; }

        // Sleeve circumference at bicep area (inches)
        [Required, Range(0, 100)]
        public decimal SleeveCircumference { get; set; }

        // Total kurta length from shoulder to hem (inches)
        [Required, Range(0, 150)]
        public decimal KurtaLength { get; set; }

        // Neck opening depth measurement (inches)
        [Required, Range(0, 50)]
        public decimal NeckDepth { get; set; }

        // Neck opening width measurement (inches)
        [Required, Range(0, 50)]
        public decimal NeckWidth { get; set; }

        // Side slit height from hem (inches)
        [Required, Range(0, 50)]
        public decimal SideSlitHeight { get; set; }

        // Navigation properties for entity relationships
        public virtual Measurement Measurement { get; set; } = null!;
        public virtual TypeModel Type { get; set; } = null!;
    }
}
