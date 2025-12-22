using Microsoft.EntityFrameworkCore;
using WebApiBoutique.Models;
using WebApiBoutique.Models.DTOs;
using WebApiBoutique.Data;

namespace WebApiBoutique.Services
{
    // Service class for basic user management operations
    public class UserService : IUserService
    {
        // Database context for user data access
        private readonly AppDbContext _context;

        // Constructor to initialize database context
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        // Retrieve all users from database
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        // Get specific user by ID
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        // Create new user in database
        public async Task<User> CreateUserAsync(User user)
        {
            // Add user to database context
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // Update existing user with new information
        public async Task<bool> UpdateUserAsync(User user)
        {
            // Find existing user in database
            var existingUser = await _context.Users.FindAsync(user.UserId);
            if (existingUser == null) return false;

            // Update user properties
            existingUser.Role = user.Role;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Name = user.Name;
            existingUser.Active = user.Active;
            existingUser.ConfirmationToken = user.ConfirmationToken;
            existingUser.IsConfirmed = user.IsConfirmed;

            await _context.SaveChangesAsync();
            return true;
        }

        // Delete user from database (hard delete)
        public async Task<bool> DeleteUserAsync(int id)
        {
            // Find user to delete
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            // Remove user from database
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // Get password change history for a user using stored procedure
        public async Task<List<PasswordHistoryDTO>> GetPasswordHistoryByUserIdAsync(int userId)
        {
            // Execute stored procedure to get password history
            return await _context.Set<PasswordHistoryDTO>()
                .FromSqlInterpolated($"EXEC GetPasswordHistoryByUserId {userId}")
                .ToListAsync();
        }
    }
}
