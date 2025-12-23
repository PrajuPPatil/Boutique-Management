using WebApiBoutique.Models;
using WebApiBoutique.Models.DTOs;

namespace WebApiBoutique.Services
{
    public interface IMeasurementService
    {
        Task<List<MeasurementDto>> GetMeasurementsByCustomerAsync(int customerId, int businessId);
        Task<MeasurementDto?> GetMeasurementByIdAsync(int id);
        Task<bool> CreateMeasurementAsync(Measurement measurement);
        Task<bool> UpdateMeasurementAsync(int id, Measurement measurement);
        Task<bool> DeleteMeasurementAsync(int id);
        Task<ValidationResult> ValidateMeasurementAsync(string gender, string measurementType, decimal value);
        Task<MeasurementStatisticsDto> GetMeasurementStatisticsAsync();
        Task<List<RecentMeasurementDto>> GetRecentMeasurementsAsync(int limit = 10);
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}