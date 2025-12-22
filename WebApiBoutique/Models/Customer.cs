using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApiBoutique.Models
{
    // Entity model representing a customer in the boutique system
    public class Customer
    {
        // Primary key for customer identification (global unique)
        [Key]
        public int CustomerId { get; set; }

        // Customer's full name with validation
        [Required, StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        // Email address with format validation and uniqueness constraint
        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        // Phone number for contact purposes
        [Required, StringLength(15)]
        public string PhoneNo { get; set; } = string.Empty;

        // Customer's physical address
        [Required, StringLength(200)]
        public string Address { get; set; } = string.Empty;

        // Gender category for measurement purposes (Men, Women, Kids)
        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;

        // Timestamp when customer record was created
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Soft delete flag - false means customer is deactivated
        public bool Active { get; set; } = true;
        
        // Multi-tenant support - each customer belongs to a business
        public int BusinessId { get; set; } = 1;

        // Navigation properties for related entities (ignored in JSON serialization)
        [JsonIgnore]
        public ICollection<Measurement> Measurements { get; set; } = new List<Measurement>();  // Customer's measurements
        [JsonIgnore]
        public ICollection<Delivery_Status> Deliveries { get; set; } = new List<Delivery_Status>();  // Delivery records
        [JsonIgnore]
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();  // Payment history
    }
}
