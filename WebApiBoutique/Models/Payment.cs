using System.ComponentModel.DataAnnotations;

namespace WebApiBoutique.Models
{
    // Entity model representing a payment transaction in the boutique system
    public class Payment
    {
        // Primary key for payment identification
        [Key]
        public int PaymentId { get; set; }

        // Foreign key linking to the order being paid for
        [Required]
        public int OrderId { get; set; }

        // Payment amount with validation to ensure positive values
        [Required, Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        // Payment method used (Cash, Card, UPI, BankTransfer, Cheque)
        [Required, StringLength(50)]
        public string PaymentMethod { get; set; } = "Cash"; // Default to Cash

        // Payment status tracking (Pending, Completed, Failed, Refunded)
        [Required, StringLength(20)]
        public string Status { get; set; } = "Completed"; // Default to Completed

        // Optional transaction ID from payment gateway or bank
        [StringLength(100)]
        public string? TransactionId { get; set; }

        // Date and time when payment was made
        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        // Optional notes or remarks about the payment
        [StringLength(500)]
        public string? Notes { get; set; }

        // Timestamp when payment record was created in system
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Multi-tenant support
        public int BusinessId { get; set; } = 1;

        // Navigation property to associated order
        public virtual Order Order { get; set; } = null!;
    }
}
