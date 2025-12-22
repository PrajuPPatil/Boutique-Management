using Microsoft.EntityFrameworkCore;

namespace WebApiBoutique.Models.DTOs
{
    [Keyless]
    public class CustomerPaymentDTO
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int PaymentId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal BalanceAmount { get; set; }
    }
}