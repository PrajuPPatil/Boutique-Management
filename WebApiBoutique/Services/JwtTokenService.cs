using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApiBoutique.Auth;
using WebApiBoutique.Models;
using WebApiBoutique.Services.Interface;

namespace WebApiBoutique.Services
{
    // Service class for JWT token generation and validation
    public class JwtTokenService : IJwtTokenService
    {
        // JWT configuration options (key, issuer, audience, expiry)
        private readonly JwtOptions _options;

        // Constructor to initialize JWT configuration
        public JwtTokenService(IOptions<JwtOptions> options)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        // Generate JWT token for authenticated user with claims
        public string GenerateToken(ApplicationUser user, IEnumerable<Claim>? extraClaims = null)
        {
            // Validate required JWT configuration
            if (string.IsNullOrWhiteSpace(_options.Key))
                throw new InvalidOperationException("JWT signing key is missing.");
            if (string.IsNullOrWhiteSpace(_options.Issuer))
                throw new InvalidOperationException("JWT issuer is missing.");
            if (string.IsNullOrWhiteSpace(_options.Audience))
                throw new InvalidOperationException("JWT audience is missing.");

            // Create signing key and credentials for token security
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Build standard JWT claims with user information
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),    // Subject (user ID)
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID (unique)
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()), // Issued at
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),      // User identifier
                new(ClaimTypes.Name, user.Email ?? string.Empty),        // User name (email)
                new(ClaimTypes.Email, user.Email ?? string.Empty),       // Email address
                new(ClaimTypes.Role, user.Role ?? "User"),                // User role for authorization
                new("BusinessId", user.BusinessId.ToString()),           // Business ID for multi-tenant
                new("BusinessName", user.BusinessName ?? "Default Business") // Business name
            };

            // Add any additional custom claims if provided
            if (extraClaims != null)
                claims.AddRange(extraClaims);

            // Create JWT token with all configuration and claims
            var token = new JwtSecurityToken(
                issuer: _options.Issuer,                                    // Token issuer
                audience: _options.Audience,                                // Token audience
                claims: claims,                                             // User claims
                expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes), // Expiration time
                signingCredentials: creds                                   // Signing credentials
            );

            // Serialize token to string format
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Validate JWT token signature and claims
        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_options.Key);

            try
            {
                // Validate token with strict security parameters
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,                    // Verify signature
                    IssuerSigningKey = new SymmetricSecurityKey(key),   // Signing key
                    ValidateIssuer = true,                             // Verify issuer
                    ValidIssuer = _options.Issuer,                     // Expected issuer
                    ValidateAudience = true,                           // Verify audience
                    ValidAudience = _options.Audience,                 // Expected audience
                    ClockSkew = TimeSpan.Zero                          // No time tolerance
                }, out SecurityToken validatedToken);

                return true;  // Token is valid
            }
            catch
            {
                return false; // Token validation failed
            }
        }
    }
}

