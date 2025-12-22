using System.ComponentModel.DataAnnotations;

namespace Boutique.Client.Models.DTOs.Women
{
    public class PayjamaWomenDto
    {
        public int PayjamaWomenId { get; set; }
        public int MeasurementId { get; set; }
        public int TypeId { get; set; }


        [Required, Range(0, 200)]
        public decimal WaistCircumference { get; set; }

        [Required, Range(0, 200)]
        public decimal HipCircumference { get; set; }

        [Required, Range(0, 150)]
        public decimal ThighCircumference { get; set; }

        [Required, Range(0, 100)]
        public decimal KneeCircumference { get; set; }

        [Required, Range(0, 100)]
        public decimal CalfCircumference { get; set; }

        [Required, Range(0, 100)]
        public decimal BottomOpening { get; set; }

        [Required, Range(0, 150)]
        public decimal InseamLength { get; set; }

        [Required, Range(0, 150)]
        public decimal OutseamLength { get; set; }

        [Required, Range(0, 100)]
        public decimal CrotchDepth { get; set; }

        // // Navigation properties
        // public virtual Measurement Measurement { get; set; } = null!;
        // public virtual TypeModel Type { get; set; } = null!;
    }
}
