using System.ComponentModel.DataAnnotations;
namespace WebApiBoutique.Models.DTOs
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        
        [Required]
        public string CustomerName { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PhoneNo { get; set; } = string.Empty;
        
        [Required]
        public string Address { get; set; } = string.Empty;
        
        public string Gender { get; set; } = "Men";
        
        public DateTime CreatedDate { get; set; }
        
        public bool Active { get; set; }
    }
}
