using System.ComponentModel.DataAnnotations;

namespace Boutique.Client.Models.DTOs.Women
{
    public class FrockDto
    {
        public int FrockId { get; set; }
        public int MeasurementId { get; set; }
        public int TypeId { get; set; }

        [Required]
        [Range(0, 200)]
        public decimal Bust { get; set; }

        [Required]
        [Range(0, 200)]
        public decimal Waist { get; set; }

        [Required]
        [Range(0, 200)]
        public decimal Hip { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal ShoulderWidth { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal Armhole { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal SleeveLength { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal SleeveCircumference { get; set; }

        [Required]
        [Range(0, 50)]
        public decimal NeckDepthFront { get; set; }

        [Required]
        [Range(0, 50)]
        public decimal NeckDepthBack { get; set; }

        [Required]
        [Range(0, 50)]
        public decimal NeckWidth { get; set; }

        [Required]
        [Range(0, 150)]
        public decimal FrockLength { get; set; }

        [Required]
        [Range(0, 200)]
        public decimal FlareWidth { get; set; }

        [Required]
        [Range(0, 50)]
        public decimal YokeDepth { get; set; }
    }
}