using Microsoft.EntityFrameworkCore;
using WebApiBoutique.Models;
using WebApiBoutique.Models.DTOs;
using WebApiBoutique.Data;

namespace WebApiBoutique.Services
{
    // Service class implementing customer business logic and data operations
    public class CustomerService : ICustomerService
    {
        // Database context for customer data access
        private readonly AppDbContext _context;

        // Constructor to initialize database context
        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        // Retrieve customers by business ID for multi-tenant support
        public async Task<IEnumerable<Customer>> GetCustomersByBusinessIdAsync(int businessId)
        {
            Console.WriteLine($"CustomerService: GetCustomersByBusinessIdAsync called for business {businessId}");
            try
            {
                var customers = await _context.Customers
                    .AsNoTracking()
                    .Where(c => c.BusinessId == businessId)  // Filter by business ID
                    .Select(c => new Customer
                    {
                        CustomerId = c.CustomerId,
                        CustomerName = c.CustomerName ?? "",
                        Email = c.Email ?? "",
                        PhoneNo = c.PhoneNo ?? "",
                        Address = c.Address ?? "",
                        Gender = c.Gender,
                        CreatedDate = c.CreatedDate == default ? DateTime.UtcNow : c.CreatedDate,
                        Active = c.Active,
                        BusinessId = c.BusinessId
                    })
                    .ToListAsync();
                
                Console.WriteLine($"CustomerService: Retrieved {customers.Count} customers for business {businessId}");
                return customers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CustomerService Error: {ex.Message}");
                throw;
            }
        }

        // Get specific customer by ID
        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            // Find customer by primary key with no tracking for read-only operation
            return await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CustomerId == id);
        }

        // public async Task<object> CreateCustomerAsync(Customer customer)
        // {
        //     if (customer == null) throw new ArgumentNullException(nameof(customer));

        //     _context.Customers.Add(customer);
        //     await _context.SaveChangesAsync();

        //     return new
        //     {
        //         customer.CustomerId,
        //         customer.CustomerName,
        //         customer.Email,
        //         customer.PhoneNo,
        //         customer.Address,
        //         customer.Active
        //     };
        // }

        // Create new customer with validation and default values
        public async Task<CustomerDto> CreateCustomerAsync(Customer customer)
        {
            try
            {
                // Set default values for required fields if not provided
                if (customer.CreatedDate == default)
                    customer.CreatedDate = DateTime.UtcNow;

                // Add customer to database context
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                return new CustomerDto
                {
                    CustomerId = customer.CustomerId,
                    CustomerName = customer.CustomerName,
                    Email = customer.Email,
                    PhoneNo = customer.PhoneNo,
                    Address = customer.Address,
                    Gender = customer.Gender,
                    CreatedDate = customer.CreatedDate,
                    Active = customer.Active
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateCustomerAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        // Update existing customer information
        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            // Validate input parameter
            if (customer == null) return false;

            // Find existing customer in database
            var existingCustomer = await _context.Customers.FindAsync(customer.CustomerId);
            if (existingCustomer == null) return false;
            
            Console.WriteLine(existingCustomer);
            // Update all properties of existing customer with new values
            _context.Entry(existingCustomer).CurrentValues.SetValues(customer);
            await _context.SaveChangesAsync();  // Save changes to database
            return true;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return false;

            // Check for related records that would prevent deletion
            var hasOrders = await _context.Orders.AnyAsync(o => o.CustomerId == id);
            var hasMeasurements = await _context.Measurements.AnyAsync(m => m.CustomerId == id);
            var hasPayments = await _context.Orders
                .Where(o => o.CustomerId == id)
                .SelectMany(o => o.Payments)
                .AnyAsync();
            
            if (hasOrders || hasMeasurements || hasPayments)
            {
                throw new InvalidOperationException("Cannot delete customer with existing orders, measurements, or payments. Please remove related records first.");
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        // Get comprehensive customer measurement details using stored procedure
        public async Task<List<CustomerMeasurementDTO>> GetCustomerMeasurementDetailsAsync()
        {
            // Execute stored procedure to get customer measurement data
            return await _context.GetCustomerMeasurementDetailsAsync();
        }
       
        public async Task<List<CustomerDeliveryDTO>> GetCustomerDeliveryDetailsAsync()
        {
            var dtoList = await _context.GetCustomerDeliveryDetailsAsync(); 
            Console.WriteLine($"Returned {dtoList.Count} rows");
            return dtoList;
        }

        public async Task<List<CustomerDeliveryDTO>> GetCustomerDeliveryDetailsByNameAsync(string name)
        {
            var dtoList = await _context.GetCustomerDeliveryDetailsByNameAsync(name);
            Console.WriteLine($"Returned {dtoList.Count} rows");
            return dtoList;
        }
       
        // Search customer measurements by customer name using stored procedure
        public async Task<List<CustomerMeasurementDTO>> GetCustomerMeasurementDetailsByNameAsync(string name)
        {
            // Execute parameterized stored procedure to prevent SQL injection
            return await _context.Set<CustomerMeasurementDTO>()
                .FromSqlInterpolated($"EXEC GetCustomerMeasurementDetailsByName {name}")
                .ToListAsync();
        }

        public async Task<List<CustomerMeasurementDTO>> GetCustomerMeasurementDetailsByNameAndTypeAsync(string name, string type)
        {
            return await _context.Set<CustomerMeasurementDTO>()
                .FromSqlInterpolated($"EXEC GetCustomerMeasurementDetailsByNameAndType {name}, {type}")
                .ToListAsync();
        }

        public async Task<List<CustomerPaymentDTO>> GetCustomerPaymentsByNameAsync(string name)
        {
            return await _context.Set<CustomerPaymentDTO>()
                .FromSqlInterpolated($"EXEC GetCustomerPaymentsByName {name}")
                .ToListAsync();
        }

        // Find customer by email address (case-insensitive search) with business filtering
        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            try
            {
                return await _context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCustomerByEmailAsync: {ex.Message}");
                return null;
            }
        }

        // Find customer by phone number for duplicate checking with business filtering
        public async Task<Customer?> GetCustomerByPhoneAsync(string phone)
        {
            try
            {
                return await _context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.PhoneNo == phone);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCustomerByPhoneAsync: {ex.Message}");
                return null;
            }
        }

    }
}
