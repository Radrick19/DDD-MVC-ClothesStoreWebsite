using Castle.Core.Smtp;
using Hangfire;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using MimeKit;
using SHA3.Net;
using Store.Application.Services.DatabaseCleanerService;
using Store.Domain.Interfaces;
using Store.Domain.Models;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Store.Mvc.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IRepository<UserEmailConfirmationHash> _userEmailConfirmationHashRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDatabaseCleanerService _applicationCleaner;
        private readonly IConfiguration _configuration;

        public EmailService(IRepository<UserEmailConfirmationHash> userEmailConfirmationHashRepository, IUnitOfWork unitOfWork, IDatabaseCleanerService applicationCleaner,
            IConfiguration configuration)
        {
            _userEmailConfirmationHashRepository = userEmailConfirmationHashRepository;
            _unitOfWork = unitOfWork;
            _applicationCleaner = applicationCleaner;
            _configuration = configuration;
        }

        public async Task SendEmailConfirmAsync(string email, User user)
        {
            var settings = _configuration.GetSection("EmailSettings");
            string confirmHash;
            using (var shaAlg = Sha3.Sha3256())
            {
                confirmHash = Convert.ToBase64String(shaAlg.ComputeHash(Encoding.UTF8.GetBytes(DateTime.Now.ToString()))).Replace("/", "");
            }
            await _userEmailConfirmationHashRepository.AddAsync(new UserEmailConfirmationHash { User = user, ConfirmationHash = confirmHash, CreationDate = DateTime.UtcNow });
            await _unitOfWork.SaveChangesAsync();
            string link = $"http://81.200.148.227:8080/emailconfirm/{confirmHash}";
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(settings["DisplayName"], settings["From"]));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = "Qlouni. Подтверждение почты";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"Нажимите <a href=\"{link}\"> подтвердить </a> для подтверждения почты. Если вы не проходили регистрацию, просто " +
                $"проигнорируйте это сообщение",
            };
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(settings["Host"], Convert.ToInt32(settings["Port"]), Convert.ToBoolean(settings["UseSSL"]));
                await client.AuthenticateAsync(settings["From"], settings["Password"]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            BackgroundJob.Schedule(() => _applicationCleaner.DeleteUnactivatedUser(user.Id), TimeSpan.FromMinutes(10));
        }
    }
}
