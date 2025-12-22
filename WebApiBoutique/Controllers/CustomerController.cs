using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiBoutique.Models;
using WebApiBoutique.Services;
using WebApiBoutique.Models.DTOs;
using WebApiBoutique.ViewModels;
using WebApiBoutique.Data;
using System.Linq;

namespace WebApiBoutique.Controllers
{
    // API controller for managing customer operations
    [ApiController]
    [Route("api/[controller]")]
    [Microsoft.AspNetCore.Authorization.Authorize] // Require authentication
    public class CustomerController : ControllerBase
    {
        // Dependency injection for customer service
        private readonly ICustomerService _customerService;
        private readonly AppDbContext _context;

        // Constructor to initialize customer service
        public CustomerController(ICustomerService customerService, AppDbContext context)
        {
            _customerService = customerService;
            _context = context;
        }

        // GET: api/Customer - Retrieve all customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllCustomers()
        {
            Console.WriteLine("GetAllCustomers endpoint called");
            try
            {
                // Get business ID from authenticated user's token
                var businessId = GetCurrentUserBusinessId();
                Console.WriteLine($"Getting customers for business ID: {businessId}");
                
                var customers = await _customerService.GetCustomersByBusinessIdAsync(businessId);
                
                Console.WriteLine($"Found {customers.Count()} customers");
                
                // Convert to DTOs for API response
                var customerDtos = customers.Select(c => new CustomerDto
                {
                    CustomerId = c.CustomerId,
                    CustomerName = c.CustomerName ?? "",
                    Email = c.Email ?? "",
                    PhoneNo = c.PhoneNo ?? "",
                    Address = c.Address ?? "",
                    Gender = c.Gender,
                    CreatedDate = c.CreatedDate,
                    Active = c.Active
                }).ToList();
                
                return Ok(customerDtos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting customers: {ex.Message}");
                return StatusCode(500, new { message = ex.Message });
            }
        }
        
        private int GetBusinessIdFromToken()
        {
            // Extract BusinessId from JWT token claims
            var businessIdClaim = User.FindFirst("BusinessId")?.Value;
            if (int.TryParse(businessIdClaim, out var businessId))
                return businessId;
            
            // Fallback to default business ID for development
            return 1;
        }
        
        private int GetCurrentUserBusinessId()
        {
            // For development/testing, return default business ID if no auth
            if (!User.Identity?.IsAuthenticated ?? true)
                return 1;
                
            return GetBusinessIdFromToken();
        }

        [HttpGet("test")]
        [AllowAnonymous] // Allow anonymous access for testing
        public IActionResult Test()
        {
            Console.WriteLine("Test endpoint called");
            return Ok("API is working");
        }

        // GET: api/Customer/{id} - Get customer by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
        {
            // Fetch customer by ID from database
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound();

            // Convert to DTO for API response
            var customerDto = new CustomerDto
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName ?? "",
                Email = customer.Email ?? "",
                PhoneNo = customer.PhoneNo ?? "",
                Address = customer.Address ?? "",
                Gender = customer.Gender, // Use actual gender
                CreatedDate = customer.CreatedDate,
                Active = customer.Active
            };

            return Ok(customerDto);
        }

                
        // POST: api/Customer - Create a new customer
        [HttpPost]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<CustomerDto>> CreateCustomer([FromBody] Customer customer)
        {
            try
            {
                if (customer == null)
                    return BadRequest(new { message = "Customer data is required", success = false });

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage));
                    return BadRequest(new { message = string.Join("; ", errors), success = false });
                }

                // Validation
                if (string.IsNullOrWhiteSpace(customer.CustomerName))
                    return BadRequest(new { message = "Customer name is required", success = false });
                
                if (string.IsNullOrWhiteSpace(customer.Email) || !IsValidEmail(customer.Email))
                    return BadRequest(new { message = "Valid email is required", success = false });
                
                if (string.IsNullOrWhiteSpace(customer.PhoneNo) || !IsValidPhoneNumber(customer.PhoneNo))
                    return BadRequest(new { message = "Valid 10-digit phone number is required", success = false });

                // Check duplicates
                var businessId = GetCurrentUserBusinessId();
                var emailExists = await _context.Customers.AnyAsync(c => c.Email.ToLower() == customer.Email.ToLower() && c.BusinessId == businessId);
                if (emailExists)
                    return BadRequest(new { message = "Email already exists", success = false });

                var phoneExists = await _context.Customers.AnyAsync(c => c.PhoneNo == customer.PhoneNo && c.BusinessId == businessId);
                if (phoneExists)
                    return BadRequest(new { message = "Phone number already exists", success = false });

                customer.BusinessId = businessId;
                var createdCustomer = await _customerService.CreateCustomerAsync(customer);
                
                return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.CustomerId }, new { 
                    message = "Customer created successfully", 
                    success = true, 
                    customer = createdCustomer 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to create customer: {ex.Message}", success = false });
            }
        }
        
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;
                
            // Remove any non-digit characters
            var digitsOnly = new string(phoneNumber.Where(char.IsDigit).ToArray());
            
            // Check if it's exactly 10 digits
            return digitsOnly.Length == 10;
        }

        // GET: api/Customer/check-duplicate - Check if email or phone already exists
        [HttpGet("check-duplicate")]
        public async Task<ActionResult> CheckDuplicate([FromQuery] string? email, [FromQuery] string? phone)
        {
            try
            {
                var businessId = GetCurrentUserBusinessId();
                var result = new { emailExists = false, phoneExists = false };
                
                if (!string.IsNullOrEmpty(email))
                {
                    var emailExists = await _context.Customers
                        .AnyAsync(c => c.Email.ToLower() == email.ToLower() && c.BusinessId == businessId);
                    result = new { emailExists, phoneExists = result.phoneExists };
                }
                
                if (!string.IsNullOrEmpty(phone))
                {
                    var phoneExists = await _context.Customers
                        .AnyAsync(c => c.PhoneNo == phone && c.BusinessId == businessId);
                    result = new { emailExists = result.emailExists, phoneExists };
                }
                
                return Ok(result);
            }
            catch
            {
                return Ok(new { emailExists = false, phoneExists = false });
            }
        }



        // PUT: api/Customer/{id} - Update existing customer
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer customer)
        {
            // Validate that URL ID matches customer ID
            if (id != customer.CustomerId)
                return BadRequest();

            // Update customer in database
            var updated = await _customerService.UpdateCustomerAsync(customer);
            if (!updated)
                return NotFound();

            return Ok("User Updated Successfully");
        }

        // DELETE: api/Customer/{id} - Delete customer by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                // Delete customer from database
                var deleted = await _customerService.DeleteCustomerAsync(id);
                if (!deleted)
                    return NotFound("Customer not found");

                return Ok("Customer deleted successfully");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message, success = false });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to delete customer", success = false });
            }
        }

        // GET: api/Customer/customer-measurements - Get all customer measurement details
        [HttpGet("customer-measurements")]
        public async Task<IActionResult> GetCustomerMeasurementDetailsAsync()
        {
            // Fetch customer measurement details from database
            var result = await _customerService.GetCustomerMeasurementDetailsAsync();
            return Ok(result);
        }

        // GET: api/Customer/measurement-details-by-name - Get measurement details by customer name
        [HttpGet("measurement-details-by-name")]
        public async Task<IActionResult> GetMeasurementDetailsByName([FromQuery] string name)
        {
            // Search for customer measurements by name
            var results = await _customerService.GetCustomerMeasurementDetailsByNameAsync(name);
            if (results == null || !results.Any())
            {
                return NotFound("Name doesn't exist, please enter a valid name");
            }
            return Ok(results);
        }


        // [HttpGet("test-measurements")]
        // public async Task<IActionResult> TestMeasurementProcedure()
        // {
        //     var result = await _customerService.GetCustomerMeasurementDetailsByNameAsync("ravi");

        //     foreach (var item in result)
        //     {
        //         Console.WriteLine($"{item.CustomerName} - {item.TypeName}");
        //     }

        //     return Ok(result);
        // }

        [HttpGet("delivery-details")]
        public async Task<IActionResult> GetCustomerDeliveryDetails()
        {
            var dtoList = await _customerService.GetCustomerDeliveryDetailsAsync();
            Console.WriteLine($"Returned {dtoList.Count} rows");
            var viewModelList = dtoList.Select(dto => new CustomerDeliveryDetailsViewModel
            {
                CustomerId = dto.CustomerId,
                CustomerName = dto.CustomerName,
                Email = dto.Email,
                PhoneNo = dto.PhoneNo,
                DeliveryId = dto.DeliveryId,
                Status = dto.Status,
                EntryDate = dto.EntryDate,
                AdvanceAmount = dto.AdvanceAmount,
                AdvanceDate = dto.AdvanceDate,
                PaymentStatus = dto.PaymentStatus
            }).ToList();

            return Ok(viewModelList);
        }

        [HttpGet("delivery-details-by-name")]
        public async Task<IActionResult> GetCustomerDeliveryDetailsByName([FromQuery] string name)
        {
            var dtoList = await _customerService.GetCustomerDeliveryDetailsByNameAsync(name);
             if (dtoList == null || !dtoList.Any())
                {
                    return Ok("Name doesn't exist, please enter a valid name");
                }
            var viewModelList = dtoList.Select(dto => new CustomerDeliveryDetailsViewModel
            {
                CustomerId = dto.CustomerId,
                CustomerName = dto.CustomerName,
                Email = dto.Email,
                PhoneNo = dto.PhoneNo,
                DeliveryId = dto.DeliveryId,
                Status = dto.Status,
                EntryDate = dto.EntryDate,
                AdvanceAmount = dto.AdvanceAmount,
                AdvanceDate = dto.AdvanceDate,
                PaymentStatus = dto.PaymentStatus
            }).ToList();

            return Ok(viewModelList);
        }
       
       [HttpGet("measurement-details-by-name-and-type")]
        public async Task<IActionResult> GetMeasurementDetailsByNameAndType([FromQuery] string name, [FromQuery] string type)
        {
            var results = await _customerService.GetCustomerMeasurementDetailsByNameAndTypeAsync(name, type);

            if (results == null || !results.Any())
                return NotFound("No matching customer and type found. Please enter valid values.");

            var viewModels = results.Select(dto => new CustomerMeasurementViewModel
            {
                CustomerId = dto.CustomerId,
                CustomerName = dto.CustomerName,
                Email = dto.Email,
                PhoneNo = dto.PhoneNo,
                MeasurementId = dto.MeasurementId,
                FabricImage = dto.FabricImage,
                FabricColor = dto.FabricColor,
                EntryDate = dto.EntryDate,
                TypeName = dto.TypeName
            }).ToList();

            return Ok(viewModels);
        }

        [HttpGet("payment-details-by-name")]
        public async Task<IActionResult> GetCustomerPaymentsByName([FromQuery] string name)
        {
            var results = await _customerService.GetCustomerPaymentsByNameAsync(name);

            if (results == null || !results.Any())
                return NotFound("No payments found for the given customer name.");

            var viewModels = results.Select(dto => new CustomerPaymentViewModel
            {
                CustomerId = dto.CustomerId,
                CustomerName = dto.CustomerName,
                PaymentId = dto.PaymentId,
                TotalAmount = dto.TotalAmount,
                ReceivedAmount = dto.ReceivedAmount,
                BalanceAmount = dto.BalanceAmount
            }).ToList();

            return Ok(viewModels);
        }

    }
}
