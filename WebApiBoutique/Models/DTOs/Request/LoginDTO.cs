using System.ComponentModel.DataAnnotations;

namespace WebApiBoutique.Models.DTOs
{
    // Data Transfer Object for user login requests
    public class LoginDTO
    {
        // Email address used as username with validation
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        // Password for authentication (validated in service)
        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
