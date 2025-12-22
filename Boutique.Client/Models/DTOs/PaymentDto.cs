using System.ComponentModel.DataAnnotations;

namespace Boutique.Client.Models.DTOs
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

        [Required]
        public string PaymentMethod { get; set; } = "Cash";

        public string? TransactionId { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        public string? Notes { get; set; }
    }

    public class UpdatePaymentDto
    {
        public decimal? Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Status { get; set; }
        public string? TransactionId { get; set; }
        public DateTime? PaymentDate { get; set; }
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
