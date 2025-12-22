using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiBoutique.Models
{
    // Entity for women's kurti measurements with traditional Indian garment specifications
    public class W_KurtiWomen
    {
        // Primary key for kurti measurement record
        [Key]
        public int KurtiWomenId { get; set; }

        // Foreign key linking to base measurement record
        [ForeignKey(nameof(Measurement))]
        public int MeasurementId { get; set; }

        // Foreign key linking to garment type definition
        [ForeignKey(nameof(Type))]
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

        // Armhole circumference measurement (inches)
        [Required, Range(0, 100)]
        public decimal Armhole { get; set; }

        // Sleeve length from shoulder to wrist (inches)
        [Required, Range(0, 100)]
        public decimal SleeveLength { get; set; }

        // Total kurti length from shoulder to hem (inches)
        [Required, Range(0, 150)]
        public decimal KurtiLength { get; set; }

        // Front neck opening depth (inches)
        [Required, Range(0, 50)]
        public decimal NeckDepthFront { get; set; }

        // Back neck opening depth (inches)
        [Required, Range(0, 50)]
        public decimal NeckDepthBack { get; set; }

        // Neck opening width measurement (inches)
        [Required, Range(0, 50)]
        public decimal NeckWidth { get; set; }

        // Distance from shoulder to bust point (inches)
        [Required, Range(0, 100)]
        public decimal ShoulderToBust { get; set; }

        // Distance from shoulder to waist (inches)
        [Required, Range(0, 100)]
        public decimal ShoulderToWaist { get; set; }

        // Sleeve circumference at upper arm (inches)
        [Required, Range(0, 100)]
        public decimal SleeveCircumference { get; set; }

        // Side slit height from hem (inches)
        [Required, Range(0, 100)]
        public decimal SideSlitHeight { get; set; }

        // Navigation properties for entity relationships
        public virtual Measurement Measurement { get; set; } = null!;
        public virtual TypeModel Type { get; set; } = null!;
    }
}
