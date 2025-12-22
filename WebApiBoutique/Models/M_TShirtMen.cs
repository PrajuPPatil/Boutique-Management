using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiBoutique.Models
{
    public class M_TShirtMen
    {
        [Key]
        public int TShirtMenId { get; set; }

        [ForeignKey(nameof(Measurement))]
        public int MeasurementId { get; set; }

        [ForeignKey(nameof(Type))]
        public int TypeId { get; set; }

        [Required, Range(0, 200)]
        public decimal Chest { get; set; }

        [Required, Range(0, 200)]
        public decimal Waist { get; set; }

        [Required, Range(0, 100)]
        public decimal ShoulderWidth { get; set; }

        [Required, Range(0, 100)]
        public decimal SleeveLength { get; set; }

        [Required, Range(0, 100)]
        public decimal Armhole { get; set; }

        [Required, Range(0, 100)]
        public decimal SleeveCircumference { get; set; }

        [Required, Range(0, 150)]
        public decimal TShirtLength { get; set; }

        [Required, Range(0, 50)]
        public decimal NeckWidth { get; set; }

        // Navigation properties
        public virtual Measurement Measurement { get; set; } = null!;
        public virtual TypeModel Type { get; set; } = null!;
    }
}
