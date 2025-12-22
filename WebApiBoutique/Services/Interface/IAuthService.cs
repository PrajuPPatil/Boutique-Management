using System.Threading.Tasks;
using WebApiBoutique.Models.DTOs;

namespace WebApiBoutique.Services.Interface
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDTO dto, string scheme, string host);
        Task<string?> VerifyEmailAsync(string token);
        Task<string?> VerifyOtpAsync(OtpVerifyDTO dto);
        Task<string?> ConfirmEmailAsync(string email, string token);
        Task<string?> ResendConfirmationAsync(string email, string scheme, string host);

        Task<(string? error, object? result)> LoginAsync(LoginDTO dto);
        Task ForgotPasswordAsync(string email, string scheme, string host);
        Task<string?> ResetPasswordAsync(ResetPasswordDTO dto);
        Task<bool> ValidateResetTokenAsync(string email, string token);
        Task<UserProfileDTO?> GetUserProfileAsync(string email);
    }
}
