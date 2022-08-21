using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Business.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}