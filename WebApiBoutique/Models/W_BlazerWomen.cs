using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiBoutique.Models
{
    // Entity for women's blazer measurements with professional/formal specifications
    public class W_BlazerWomen
    {
        // Primary key for blazer measurement record
        [Key]
        public int BlazerWomenId { get; set; }

        // Foreign key linking to base measurement record
        [Required]
        public int MeasurementId { get; set; }

        // Foreign key linking to garment type definition
        [Required]
        public int TypeId { get; set; }

        // Bust circumference measurement (inches)
        [Required, Range(0, 200)]
        public decimal Bust { get; set; }

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

        // Sleeve circumference at upper arm (inches)
        [Required, Range(0, 100)]
        public decimal SleeveCircumference { get; set; }

        // Total blazer length from shoulder to hem (inches)
        [Required, Range(0, 150)]
        public decimal BlazerLength { get; set; }

        // Neck opening width measurement (inches)
        [Required, Range(0, 50)]
        public decimal NeckWidth { get; set; }

        // Back width across shoulder blades (inches)
        [Required, Range(0, 100)]
        public decimal BackWidth { get; set; }

        // Lapel depth for professional styling (inches)
        [Required, Range(0, 50)]
        public decimal LapelDepth { get; set; }

        // Navigation properties with explicit foreign key attributes
        [ForeignKey(nameof(MeasurementId))]
        public Measurement Measurement { get; set; } = null!;

        [ForeignKey(nameof(TypeId))]
        public TypeModel Type { get; set; } = null!;
    }
}
