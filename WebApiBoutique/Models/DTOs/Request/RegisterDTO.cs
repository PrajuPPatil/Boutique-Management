using System.ComponentModel.DataAnnotations;

namespace WebApiBoutique.Models.DTOs
{
    // Data Transfer Object for user registration requests
    public class RegisterDTO
    {
        // Username for display purposes with length validation
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        // Email address with format validation (used for login)
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        // Password with security requirements (validated in service)
        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        // Confirm password field for validation
        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;

        // Email verification method: "otp" or "link" (defaults to OTP)
        public string? VerificationMethod { get; set; } = "otp";


    }
}
