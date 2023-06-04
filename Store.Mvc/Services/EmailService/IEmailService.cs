using Store.Domain.Models;

namespace Store.Mvc.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmailConfirmAsync(string email, User user);
    }
}
