namespace Boutique.Client.Models.DTOs
{
    public class CustomerPaymentDTO
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int PaymentId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal BalanceAmount { get; set; }

        // Computed properties for UI
        public decimal PaidAmount => TotalAmount - BalanceAmount;
        public bool IsFullyPaid => BalanceAmount <= 0;
        public decimal PaymentPercentage => TotalAmount > 0 ? ((TotalAmount - BalanceAmount) / TotalAmount) * 100 : 0;
    }
}