using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiBoutique.Models
{
    // Entity for men's shirt measurements with formal/casual shirt specifications
    public class M_ShirtMen
    {
        // Primary key for shirt measurement record
        [Key]
        public int ShirtMenId { get; set; }

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

        // Total shirt length from shoulder to hem (inches)
        [Required, Range(0, 150)]
        public decimal ShirtLength { get; set; }

        // Neck circumference for collar sizing (inches)
        [Required, Range(0, 50)]
        public decimal NeckCircumference { get; set; }

        // Cuff circumference for wrist fit (inches)
        [Required, Range(0, 50)]
        public decimal CuffCircumference { get; set; }

        // Back width across shoulder blades (inches)
        [Required, Range(0, 100)]
        public decimal BackWidth { get; set; }

        // Navigation properties for entity relationships
        public virtual Measurement Measurement { get; set; } = null!;
        public virtual TypeModel Type { get; set; } = null!;
    }
}
