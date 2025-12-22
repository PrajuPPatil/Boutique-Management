using WebApiBoutique.Models;

namespace WebApiBoutique.Services.Interface
{
    public interface IMeasurementValidationService
    {
        Task<bool> ValidateMeasurementAsync(string gender, string measurementType, decimal value);
        Task<string> GetValidationErrorMessageAsync(string gender, string measurementType, decimal value);
        Task<(decimal min, decimal max)> GetMeasurementRangeAsync(string gender, string measurementType);
        Task<bool> InsertValidatedMeasurementAsync(CustomerMeasurement measurement);
    }
}