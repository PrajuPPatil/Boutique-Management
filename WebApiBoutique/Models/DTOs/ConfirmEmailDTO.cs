using System.ComponentModel.DataAnnotations;

namespace WebApiBoutique.Models.DTOs
{
    // Data Transfer Object for email confirmation requests
    public class ConfirmEmailDTO
    {
        // Email address to confirm with validation
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

       
    }
}
