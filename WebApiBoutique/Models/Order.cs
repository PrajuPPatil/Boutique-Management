using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiBoutique.Models
{
    // Entity model representing a customer order in the boutique system
    public class Order
    {
        // Primary key for order identification
        [Key]
        public int OrderId { get; set; }

        // Foreign key linking to customer who placed the order
        [Required]
        public int CustomerId { get; set; }

        // Optional foreign key linking to measurement record
        public int? MeasurementId { get; set; }

        // Order status tracking (Pending, InProgress, ReadyForDelivery, Delivered)
        [Required, StringLength(20)]
        public string Status { get; set; } = "Pending"; // Default to Pending status

        // Priority level affecting delivery timeframe (Regular, Urgent, Express)
        [Required, StringLength(10)]
        public string Priority { get; set; } = "Regular"; // Default to Regular priority

        // Timestamp when order was placed
        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        // Calculated delivery date based on priority
        [Required]
        public DateTime EstimatedDeliveryDate { get; set; }

        // Actual delivery date (set when status becomes Delivered)
        public DateTime? ActualDeliveryDate { get; set; }

        // Optional notes or special instructions for the order
        [StringLength(500)]
        public string? Notes { get; set; }

        // Total order amount with 2 decimal precision
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        // Amount already paid by customer
        [Column(TypeName = "decimal(10,2)")]
        public decimal PaidAmount { get; set; }

        // Calculated remaining balance (computed property)
        [Column(TypeName = "decimal(10,2)")]
        public decimal RemainingAmount => TotalAmount - PaidAmount;

        // Soft delete flag - false means order is deactivated
        public bool IsActive { get; set; } = true;
        
        // Multi-tenant support
        public int BusinessId { get; set; } = 1;

        // Navigation properties for related entities
        public Customer Customer { get; set; } = null!;  // Customer who placed the order
        public Measurement Measurement { get; set; } = null!;  // Associated measurement record
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();  // Payment history for this order
    }
}