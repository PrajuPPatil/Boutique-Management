using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiBoutique.Models
{
    // Entity representing delivery status tracking for customer orders
    public class Delivery_Status
    {
        // Primary key for delivery status record
        [Key]
        public int DeliveryId { get; set; }

        // Foreign key linking to customer record
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }

        // Current delivery status (Pending, InProgress, ReadyForDelivery, Delivered)
        [Required, StringLength(50)]
        public string Status { get; set; } = string.Empty;

        // Date when delivery status was recorded
        [Required]
        public DateTime EntryDate { get; set; }

        // Advance payment amount received from customer
        [Required, Range(0, double.MaxValue)]
        public decimal AdvanceAmount { get; set; }

        // Date when advance payment was received
        [Required]
        public DateTime AdvanceDate { get; set; }

        // Payment status (Pending, Completed, Partial)
        [Required, StringLength(30)]
        public string PaymentStatus { get; set; } = string.Empty;

        // Navigation property to customer entity
        public virtual Customer Customer { get; set; } = null!;
    }
}
