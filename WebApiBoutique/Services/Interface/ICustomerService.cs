using WebApiBoutique.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiBoutique.Models.DTOs;
namespace WebApiBoutique.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetCustomersByBusinessIdAsync(int businessId);
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<bool> UpdateCustomerAsync(Customer customer);
        Task<bool> DeleteCustomerAsync(int id);
        Task<CustomerDto> CreateCustomerAsync(Customer customer);
        Task<List<CustomerMeasurementDTO>> GetCustomerMeasurementDetailsAsync();
        Task<List<CustomerMeasurementDTO>> GetCustomerMeasurementDetailsByNameAsync(string name);
        Task<List<CustomerDeliveryDTO>> GetCustomerDeliveryDetailsAsync();
        Task<List<CustomerDeliveryDTO>> GetCustomerDeliveryDetailsByNameAsync(string name);
        Task<List<CustomerMeasurementDTO>> GetCustomerMeasurementDetailsByNameAndTypeAsync(string name, string type);
        Task<List<CustomerPaymentDTO>> GetCustomerPaymentsByNameAsync(string name);
        Task<Customer?> GetCustomerByEmailAsync(string email);
        Task<Customer?> GetCustomerByPhoneAsync(string phone);
    }
}
