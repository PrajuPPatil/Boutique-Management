using WebApiBoutique.Models;

namespace WebApiBoutique.Repository.Interface
{
    public interface IUserRepository
    {
        // General user operations
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(int id);

        // Authentication-related operations
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<ApplicationUser?> GetByConfirmationTokenAsync(string token);
        Task<bool> EmailExistsAsync(string email);
        Task<ApplicationUser> CreateAsync(ApplicationUser user, string password);
        Task<bool> VerifyPasswordAsync(ApplicationUser user, string password);
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
        Task<bool> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
        Task UpdateAsync(ApplicationUser user);
    }
}
