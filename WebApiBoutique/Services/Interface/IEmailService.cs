using System.Threading.Tasks;

namespace WebApiBoutique.Services.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlMessage);
    }
}