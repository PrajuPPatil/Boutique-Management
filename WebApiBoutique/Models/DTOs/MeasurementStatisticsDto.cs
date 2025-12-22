namespace WebApiBoutique.Models.DTOs
{
    public class MeasurementStatisticsDto
    {
        public int TotalMeasurements { get; set; }
        public int TotalCustomers { get; set; }
        public int MenMeasurements { get; set; }
        public int WomenMeasurements { get; set; }
        public int TodayMeasurements { get; set; }
        public Dictionary<string, int> GarmentTypeCounts { get; set; } = new();
    }

    public class RecentMeasurementDto
    {
        public int MeasurementId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string GarmentType { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public int CustomerId { get; set; }
    }
}