using System.ComponentModel.DataAnnotations;

namespace Boutique.Client.Models.DTOs
{
    public class MeasurementDto
    {
        public int MeasurementId { get; set; }
        public int CustomerId { get; set; }
        public int TypeId { get; set; }

        [Required]
        [StringLength(200)]
        public string FabricImage { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FabricColor { get; set; } = string.Empty;

        [Required]
        public DateTime EntryDate { get; set; } = DateTime.Now;

        // Navigation properties for display
        public string? CustomerName { get; set; }
        public string? TypeName { get; set; }
    }

    public class CreateMeasurementDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int TypeId { get; set; }

        [Required]
        [StringLength(200)]
        public string FabricImage { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FabricColor { get; set; } = string.Empty;

        public DateTime EntryDate { get; set; } = DateTime.Now;
    }

    public class UpdateMeasurementDto
    {
        [Required]
        [StringLength(200)]
        public string FabricImage { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FabricColor { get; set; } = string.Empty;

        [Required]
        public DateTime EntryDate { get; set; }
    }
}