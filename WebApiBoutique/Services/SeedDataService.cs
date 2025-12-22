using WebApiBoutique.Data;
using WebApiBoutique.Models;

namespace WebApiBoutique.Services
{
    public class SeedDataService
    {
        private readonly AppDbContext _context;

        public SeedDataService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Seed garment types if none exist
            if (!_context.Types.Any())
            {
                var types = new List<TypeModel>
                {
                    new() { TypeName = "Kurta" },
                    new() { TypeName = "Shirt" },
                    new() { TypeName = "Pant" },
                    new() { TypeName = "Blazer" },
                    new() { TypeName = "Kurti" },
                    new() { TypeName = "Frock" },
                    new() { TypeName = "Top" },
                    new() { TypeName = "Payjama" },
                    new() { TypeName = "T-Shirt" }
                };

                _context.Types.AddRange(types);
                await _context.SaveChangesAsync();
            }

            // Seed admin user if none exists
            if (!_context.ApplicationUsers.Any())
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@boutique.com",
                    Email = "admin@boutique.com",
                    PasswordHash = HashPassword("Admin@123"),
                    Role = "Admin",
                    IsEmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.ApplicationUsers.Add(adminUser);
                await _context.SaveChangesAsync();
            }
        }

        private static string HashPassword(string password)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.HashData(
                    System.Text.Encoding.UTF8.GetBytes(password + "BoutiqueSalt")
                )
            );
        }
    }
}