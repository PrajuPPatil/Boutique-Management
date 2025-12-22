using System.ComponentModel.DataAnnotations;

namespace WebApiBoutique.Models.DTOs
{
    public class TestEmailDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}