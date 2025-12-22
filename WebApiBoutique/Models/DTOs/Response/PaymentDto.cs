using System.ComponentModel.DataAnnotations;

namespace WebApiBoutique.Models.DTOs
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public string OrderDescription { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? TransactionId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreatePaymentDto
    {
        [Required]
        public int OrderId { get; set; }

        [Required, Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required, StringLength(50)]
        public string PaymentMethod { get; set; } = "Cash";

        [StringLength(100)]
        public string? TransactionId { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string? Notes { get; set; }
    }

    public class UpdatePaymentDto
    {
        [Range(0.01, double.MaxValue)]
        public decimal? Amount { get; set; }

        [StringLength(50)]
        public string? PaymentMethod { get; set; }

        [StringLength(20)]
        public string? Status { get; set; }

        [StringLength(100)]
        public string? TransactionId { get; set; }

        public DateTime? PaymentDate { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }

    public class PaymentSummaryDto
    {
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingBalance { get; set; }
        public int TotalOrders { get; set; }
        public int PaidOrders { get; set; }
        public int PendingOrders { get; set; }
    }
}
