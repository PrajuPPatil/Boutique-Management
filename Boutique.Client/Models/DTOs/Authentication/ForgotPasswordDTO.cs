using System.ComponentModel.DataAnnotations;

namespace Boutique.Client.Models.DTOs.Authentication
{
    public class ForgotPasswordDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
    }
}