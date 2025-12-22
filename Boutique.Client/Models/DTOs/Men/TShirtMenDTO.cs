using System.ComponentModel.DataAnnotations;

namespace Boutique.Client.Models.DTOs.Men
{
    public class TShirtMenDto
    {
        public int TShirtMenId { get; set; }
        public int MeasurementId { get; set; }
        public int TypeId { get; set; }

        [Required]
        [Range(0, 200)]
        public decimal Chest { get; set; }

        [Required]
        [Range(0, 200)]
        public decimal Waist { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal ShoulderWidth { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal SleeveLength { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal Armhole { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal SleeveCircumference { get; set; }

        [Required]
        [Range(0, 150)]
        public decimal TShirtLength { get; set; }

        [Required]
        [Range(0, 50)]
        public decimal NeckWidth { get; set; }
    }
}
