namespace Boutique.Client.Models.DTOs
{
    public class CustomerDeliveryDTO
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public int DeliveryId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime EntryDate { get; set; }
        public decimal AdvanceAmount { get; set; }
        public DateTime AdvanceDate { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
    }
}