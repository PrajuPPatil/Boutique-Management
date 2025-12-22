using System.Security.Claims;
using WebApiBoutique.Models;

namespace WebApiBoutique.Services.Interface
{
    public interface IJwtTokenService
    {
        string GenerateToken(ApplicationUser user, IEnumerable<Claim>? extraClaims = null);
        bool ValidateToken(string token);
    }
}
