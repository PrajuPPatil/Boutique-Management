using System.ComponentModel.DataAnnotations;

namespace WebApiBoutique.Models.DTOs
{
    // Data Transfer Object for forgot password requests
    public class ForgotPasswordDTO
    {
        // Email address for password reset with validation
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
    }
}
