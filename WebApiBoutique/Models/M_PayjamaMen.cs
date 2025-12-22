using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiBoutique.Models
{
    public class M_PayjamaMen
    {
        [Key]
        public int PayjamaMenId { get; set; }

        [ForeignKey(nameof(Measurement))]
        public int MeasurementId { get; set; }

        [ForeignKey(nameof(Type))]
        public int TypeId { get; set; }

        [Required, Range(0, 200)]
        public decimal Waist { get; set; }

        [Required, Range(0, 200)]
        public decimal Hip { get; set; }

        [Required, Range(0, 150)]
        public decimal Thigh { get; set; }

        [Required, Range(0, 100)]
        public decimal Knee { get; set; }

        [Required, Range(0, 100)]
        public decimal Calf { get; set; }

        [Required, Range(0, 100)]
        public decimal BottomOpening { get; set; }

        [Required, Range(0, 150)]
        public decimal InseamLength { get; set; }

        [Required, Range(0, 150)]
        public decimal OutseamLength { get; set; }

        [Required, Range(0, 100)]
        public decimal CrotchDepth { get; set; }

        // Navigation properties
        public virtual Measurement Measurement { get; set; } = null!;
        public virtual TypeModel Type { get; set; } = null!;
    }
}
