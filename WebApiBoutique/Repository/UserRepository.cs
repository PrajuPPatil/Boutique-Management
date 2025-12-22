using Microsoft.EntityFrameworkCore;
using WebApiBoutique.Models;
using WebApiBoutique.Repository.Interface;
using WebApiBoutique.Data;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace WebApiBoutique.Repository
{
    // Repository for user data access with secure password handling
    public class UserRepository : IUserRepository
    {
        // Database context for user operations
        private readonly AppDbContext _db;

        // Constructor with dependency injection for database context
        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        // Get all users from database
        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _db.ApplicationUsers.ToListAsync();
        }

        // Get user by primary key ID
        public async Task<ApplicationUser?> GetUserByIdAsync(int id)
        {
            return await _db.ApplicationUsers.FindAsync(id);
        }

        // Get user by email address (used for login)
        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            return await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == email);
        }

        // Get user by email confirmation token
        public async Task<ApplicationUser?> GetByConfirmationTokenAsync(string token)
        {
            return await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.ConfirmationToken == token);
        }

        // Check if email already exists in database
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _db.ApplicationUsers.AnyAsync(u => u.Email == email);
        }

        // Create new user with securely hashed password
        public async Task<ApplicationUser> CreateAsync(ApplicationUser user, string password)
        {
            user.PasswordHash = HashPassword(password);
            _db.ApplicationUsers.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        // Verify user password against stored hash
        public async Task<bool> VerifyPasswordAsync(ApplicationUser user, string password)
        {
            return VerifyHashedPassword(user.PasswordHash, password);
        }

        // Generate secure password reset token
        public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            return await Task.FromResult(token);
        }

        // Reset user password with token verification
        public async Task<bool> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            // Verify reset token matches
            if (user.PasswordResetToken != token)
                return false;

            // Update password with new hash
            user.PasswordHash = HashPassword(newPassword);
            await UpdateAsync(user);
            return true;
        }

        // Update user information in database
        public async Task UpdateAsync(ApplicationUser user)
        {
            _db.ApplicationUsers.Update(user);
            await _db.SaveChangesAsync();
        }

        // Hash password using PBKDF2 with random salt for security
        private static string HashPassword(string password)
        {
            // Generate random salt for each password
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            // Hash password with salt using PBKDF2 (100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            
            // Return salt.hash format for storage
            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        // Verify password against stored hash with salt
        private static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            // Split stored hash into salt and hash components
            var parts = hashedPassword.Split('.');
            if (parts.Length != 2) return false;

            // Extract salt and hash from stored value
            var salt = Convert.FromBase64String(parts[0]);
            var hash = parts[1];

            // Hash provided password with same salt
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            // Compare hashes for authentication
            return hash == hashed;
        }
    }
}
