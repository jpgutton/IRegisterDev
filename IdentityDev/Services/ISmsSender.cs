using System.Threading.Tasks;

namespace IdentityDev.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
