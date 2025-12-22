using System.ComponentModel.DataAnnotations;

namespace WebApiBoutique.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        
        [Required, StringLength(100)]
        public string UserName { get; set; } = string.Empty;
        
        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string Role { get; set; } = "User";
        
        public bool IsEmailConfirmed { get; set; } = false;
        
        // Token used for email confirmation process
        [StringLength(100)]
        public string? ConfirmationToken { get; set; }
        
        // Token used for password reset functionality
        [StringLength(100)]
        public string? PasswordResetToken { get; set; }
        
        // Expiry time for password reset token (security measure)
        public DateTime? PasswordResetTokenExpiry { get; set; }
        
        // 6-digit OTP code for email verification
        [StringLength(6)]
        public string? OtpCode { get; set; }
        
        // Timestamp when OTP was generated (for expiry checking)
        public DateTime? OtpGeneratedAt { get; set; }
        
        // Counter to track OTP resend attempts (rate limiting)
        public int OtpResendCount { get; set; } = 0;
        
        // Timestamp of last OTP resend (for rate limiting)
        public DateTime? LastOtpResendAt { get; set; }
        
        // Timestamp when user account was created
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Timestamp when user record was last updated
        public DateTime? UpdatedAt { get; set; }
        
        // Identifier of who last updated the user record
        public string? UpdatedBy { get; set; }
        
        // Multi-tenant support - each user belongs to a business
        public int BusinessId { get; set; } = 1; // Default business ID
        
        [StringLength(100)]
        public string BusinessName { get; set; } = "Default Business";
    }
}
