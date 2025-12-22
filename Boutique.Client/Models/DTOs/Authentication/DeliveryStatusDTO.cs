using System.ComponentModel.DataAnnotations;

namespace Boutique.Client.Models.DTOs
{
    public class DeliveryStatusDto
    {
        public int DeliveryId { get; set; }
        public int CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        [Required]
        public DateTime EntryDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal AdvanceAmount { get; set; }

        [Required]
        public DateTime AdvanceDate { get; set; }

        [Required]
        [StringLength(30)]
        public string PaymentStatus { get; set; } = string.Empty;

        // For display
        public string? CustomerName { get; set; }
    }

    public class CreateDeliveryStatusDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        [Required]
        public DateTime EntryDate { get; set; } = DateTime.Now;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal AdvanceAmount { get; set; }

        [Required]
        public DateTime AdvanceDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(30)]
        public string PaymentStatus { get; set; } = string.Empty;
    }

    public class UpdateDeliveryStatusDto
    {
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string PaymentStatus { get; set; } = string.Empty;
    }
}