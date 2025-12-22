using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiBoutique.Models
{
    // Entity for tracking user password change history (security feature)
    public class PasswordHistory
    {
        // Primary key for password history record
        [Key]
        public int Id { get; set; }

        // Foreign key linking to application user
        [ForeignKey("ApplicationUser")]
        public int UserId { get; set; }

        // Hashed password for security (never store plain text)
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // Timestamp when password was changed
        public DateTime ChangedAt { get; set; }

        // Navigation property to application user
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
