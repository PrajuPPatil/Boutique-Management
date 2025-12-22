using System.ComponentModel.DataAnnotations;

namespace WebApiBoutique.Models
{
    // Entity model representing a system user (different from ApplicationUser)
    public class User
    {
        // Primary key for user identification
        [Key]
        public int UserId { get; set; }

        // User role for system access control (Admin, Shopkeeper, etc.)
        [Required, StringLength(20)]
        public string Role { get; set; } = string.Empty;

        // Email address for communication and login
        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        // Phone number with validation
        [Required, Phone, StringLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;

        // Full name of the user
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Soft delete flag - false means user is deactivated
        public bool Active { get; set; } = true;

        // Token for email confirmation process
        public string? ConfirmationToken { get; set; }
        
        // Flag indicating if email has been confirmed
        public bool IsConfirmed { get; set; } 
    }
}
