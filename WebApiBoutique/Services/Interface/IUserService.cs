using WebApiBoutique.Models;
using WebApiBoutique.Models.DTOs;
using WebApiBoutique.Services;

namespace WebApiBoutique.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<List<PasswordHistoryDTO>> GetPasswordHistoryByUserIdAsync(int userId);
    }
}
