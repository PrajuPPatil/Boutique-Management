using System.ComponentModel.DataAnnotations;

namespace Boutique.Client.Models.DTOs.Men
{
    public class PantMenDto
    {
        public int PantMenId { get; set; }
        public int MeasurementId { get; set; }
        public int TypeId { get; set; }

        [Required]
        [Range(0, 200)]
        public decimal Waist { get; set; }

        [Required]
        [Range(0, 200)]
        public decimal Hip { get; set; }

        [Required]
        [Range(0, 150)]
        public decimal Thigh { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal Knee { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal Calf { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal BottomOpening { get; set; }

        [Required]
        [Range(0, 150)]
        public decimal InseamLength { get; set; }

        [Required]
        [Range(0, 150)]
        public decimal OutseamLength { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal CrotchDepth { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal FlyLength { get; set; }
    }
}