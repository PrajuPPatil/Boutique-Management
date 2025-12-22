namespace WebApiBoutique.Models.ViewModels
{
    public class GetCustomerMeasurementDetailsByNameViewModel
    {
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string FabricImage { get; set; } = string.Empty;
        public string FabricColor { get; set; } = string.Empty;
        public DateTime EntryDate { get; set; }
        public string TypeName { get; set; } = string.Empty;
    }
}
