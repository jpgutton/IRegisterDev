using System.Threading.Tasks;

namespace IdentityDev.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
