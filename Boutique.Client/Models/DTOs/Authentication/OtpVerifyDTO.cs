using System.ComponentModel.DataAnnotations;

namespace Boutique.Client.Models.DTOs.Authentication
{
    public class OtpVerifyDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "OTP code is required")]
        [StringLength(6, MinimumLength = 4, ErrorMessage = "OTP must be between 4 and 6 characters")]
        public string OtpCode { get; set; } = string.Empty;
    }
}