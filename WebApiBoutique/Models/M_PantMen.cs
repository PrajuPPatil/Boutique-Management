using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiBoutique.Models
{
    // Entity for men's pant measurements with comprehensive trouser specifications
    public class M_PantMen
    {
        // Primary key for pant measurement record
        [Key]
        public int PantMenId { get; set; }

        // Foreign key linking to base measurement record
        [ForeignKey(nameof(Measurement))]
        public int MeasurementId { get; set; }

        // Foreign key linking to garment type definition
        [ForeignKey(nameof(Type))]
        public int TypeId { get; set; }

        // Waist circumference measurement (inches)
        [Required, Range(0, 200)]
        public decimal Waist { get; set; }

        // Hip circumference measurement (inches)
        [Required, Range(0, 200)]
        public decimal Hip { get; set; }

        // Thigh circumference at widest point (inches)
        [Required, Range(0, 150)]
        public decimal Thigh { get; set; }

        // Knee circumference measurement (inches)
        [Required, Range(0, 100)]
        public decimal Knee { get; set; }

        // Calf circumference measurement (inches)
        [Required, Range(0, 100)]
        public decimal Calf { get; set; }

        // Bottom opening/hem circumference (inches)
        [Required, Range(0, 100)]
        public decimal BottomOpening { get; set; }

        // Inseam length from crotch to ankle (inches)
        [Required, Range(0, 150)]
        public decimal InseamLength { get; set; }

        // Outseam length from waist to ankle (inches)
        [Required, Range(0, 150)]
        public decimal OutseamLength { get; set; }

        // Crotch depth from waist to crotch point (inches)
        [Required, Range(0, 100)]
        public decimal CrotchDepth { get; set; }

        // Fly length for zipper placement (inches)
        [Required, Range(0, 100)]
        public decimal FlyLength { get; set; }

        // Navigation properties for entity relationships
        public virtual Measurement Measurement { get; set; } = null!;
        public virtual TypeModel Type { get; set; } = null!;
    }
}
