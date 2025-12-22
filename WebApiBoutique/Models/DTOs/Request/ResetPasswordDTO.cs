using System.ComponentModel.DataAnnotations;

namespace WebApiBoutique.Models.DTOs
{
    // Data Transfer Object for password reset requests
    public class ResetPasswordDTO
    {
        // Email address for password reset with validation
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        // Security token for password reset verification
        [Required]
        [StringLength(200)]
        public string Token { get; set; } = string.Empty;

        // New password with security requirements (validated in service)
        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
