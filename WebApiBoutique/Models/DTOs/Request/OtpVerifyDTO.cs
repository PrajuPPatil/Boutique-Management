using System.ComponentModel.DataAnnotations;

namespace WebApiBoutique.Models.DTOs
{
    public class OtpVerifyDTO
    {
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(6, MinimumLength = 4)]
        public string OtpCode { get; set; } = string.Empty;
    }
}
