using Castle.Core.Smtp;
using Hangfire;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MimeKit;
using SHA3.Net;
using Store.Application.Interfaces;
using Store.Domain.Interfaces;
using Store.Domain.Models;
using Store.MVC.Interfaces;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Store.Mvc.Infrastructure
{
    public class EmailService : IEmailService
    {
        private readonly IRepository<UserEmailConfirmationHash> _userEmailConfirmationHashRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationCleaner _applicationCleaner;

        public EmailService(IRepository<UserEmailConfirmationHash> userEmailConfirmationHashRepository, IUnitOfWork unitOfWork, IApplicationCleaner applicationCleaner)
        {
            _userEmailConfirmationHashRepository = userEmailConfirmationHashRepository;
            _unitOfWork = unitOfWork;
            _applicationCleaner = applicationCleaner;
        }

        public async Task SendEmailConfirmAsync(string email, User user)
        {
            string confirmHash;
            using (var shaAlg = Sha3.Sha3256())
            {
                confirmHash = Convert.ToBase64String(shaAlg.ComputeHash(Encoding.UTF8.GetBytes(DateTime.Now.ToString()))).Replace("/", "");
            }
            await _userEmailConfirmationHashRepository.AddAsync(new UserEmailConfirmationHash { User = user, ConfirmationHash = confirmHash, CreationDate = DateTime.UtcNow });
            await _unitOfWork.SaveChangesAsync();

            string link = $"http://localhost:5110/emailconfirm/{confirmHash}";
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Администрация сайта Клоуны", "radrick.andrey@yandex.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = "Qlouni. Подтверждение почты";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"Нажимите <a href=\"{link}\"> подтвердить </a> для подтверждения почты. Если вы не проходили регистрацию, просто " +
                $"проигнорируйте это сообщение",
            };
            using(var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.yandex.ru", 465, true);
                await client.AuthenticateAsync("radrick.andrey@yandex.ru", "mwjdvhjyagrndoyl");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            BackgroundJob.Schedule(()=> _applicationCleaner.DeleteUnactivatedUser(user.Id), TimeSpan.FromMinutes(10));
        }
    }
}
