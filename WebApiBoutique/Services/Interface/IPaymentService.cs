using WebApiBoutique.Models.DTOs;

namespace WebApiBoutique.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync();
        Task<IEnumerable<PaymentDto>> GetPaymentsByBusinessIdAsync(int businessId);
        Task<PaymentDto?> GetPaymentByIdAsync(int id);
        Task<IEnumerable<PaymentDto>> GetPaymentsByOrderAsync(int orderId);
        Task<IEnumerable<PaymentDto>> GetPaymentsByCustomerAsync(int customerId);
        Task<PaymentDto> CreatePaymentAsync(CreatePaymentDto createDto);
        Task<PaymentDto> CreatePaymentAsync(CreatePaymentDto createDto, int businessId);
        Task<bool> UpdatePaymentAsync(int id, UpdatePaymentDto updateDto);
        Task<bool> DeletePaymentAsync(int id);
        Task<PaymentSummaryDto> GetPaymentSummaryAsync(int customerId);
    }
}
