namespace WebApiBoutique.ViewModels
{
    public class CustomerMeasurementViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public int MeasurementId { get; set; }
        public string FabricImage { get; set; } = string.Empty;
        public string FabricColor { get; set; } = string.Empty;
        public DateTime EntryDate { get; set; }
        public string TypeName { get; set; } = string.Empty;
    }
}