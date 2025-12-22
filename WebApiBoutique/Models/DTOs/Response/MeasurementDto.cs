namespace WebApiBoutique.Models.DTOs
{
    public class MeasurementDto
    {
        public int MeasurementId { get; set; }
        public string TypeName { get; set; } = "";
        public string FabricColor { get; set; } = "";
        public DateTime EntryDate { get; set; }
    }
}