using Store.Domain.Models;

namespace Store.MVC.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailConfirmAsync(string email, User user);
    }
}
