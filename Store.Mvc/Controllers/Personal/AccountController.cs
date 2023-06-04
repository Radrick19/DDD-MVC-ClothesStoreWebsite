using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using SHA3.Net;
using Store.API.ViewModels.Account;
using Store.Application.Services.CaptchaValidatorService;
using Store.Domain.Enums;
using Store.Domain.Interfaces;
using Store.Domain.Models;
using Store.Mvc.Services.EmailService;
using System.Drawing.Printing;
using System.Security.Claims;
using System.Text;

namespace Store.API.Controllers.Personal
{
    public class AccountController : Controller
    {
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _context;
        private readonly IEmailService _emailService;
        private readonly ICaptchaValidatorService _captchaValidator;

        public AccountController(IRepository<User> userRepository, IUnitOfWork unitOfWork, IHttpContextAccessor context,
            IEmailService emailService, ICaptchaValidatorService captchaValidator)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _context = context;
            _emailService = emailService;
            _captchaValidator = captchaValidator;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel viewModel)
        {
            if (!await _captchaValidator.ValidateAsync(viewModel.captchaToken))
            {
                ModelState.AddModelError("", "Капча не пройдена");
                return View(viewModel);
            }
            if(_userRepository.GetQuary().Any(user=> user.Login.ToLower() == viewModel.Login.ToLower())) 
            {
                ModelState.AddModelError("Login", "Данный логин уже занят");
            }
            if (_userRepository.GetQuary().Any(user => user.Email.ToLower() == viewModel.Email.ToLower()))
            {
                ModelState.AddModelError("Email", "Данный email уже занят");
            }
            if (ModelState.IsValid)
            {
                string hash;
                string salt = DateTime.Now.GetHashCode().ToString();
                using (var shaAlg = Sha3.Sha3256())
                {
                    string inputPassHash = Convert.ToBase64String(shaAlg.ComputeHash(Encoding.UTF8.GetBytes(viewModel.Password)));
                    hash = Convert.ToBase64String(shaAlg.ComputeHash(Encoding.UTF8.GetBytes(inputPassHash + salt)));
                }
                var user = new User
                {
                    Login = viewModel.Login,
                    Email = viewModel.Email,
                    Password = hash,
                    Guid = Guid.NewGuid(),
                    Salt = salt,
                    UserRole = UserRole.Customer,
                    IsEmailConfirmed = false,
                    RegistrationDate = DateTime.UtcNow, 
                };
                await _userRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                await _emailService.SendEmailConfirmAsync(viewModel.Email, user);
                return RedirectToAction("Index", "Message", new { text = "Сообщение с подтверждением отправлено вам на почту" });
            }
            return View(viewModel);
        }

        public IActionResult Login(string returnUrl)
        {
            var viewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!await _captchaValidator.ValidateAsync(viewModel.captchaToken))
            {
                ModelState.AddModelError("", "Капча не пройдена");
            }
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            User person;
            person = await _userRepository.FirstOrDefaultAsync(user => user.Login.ToLower() == viewModel.LoginOrEmail.ToLower());
            if(person == null)
            {
                person = await _userRepository.FirstOrDefaultAsync(user => user.Email.ToLower() == viewModel.LoginOrEmail.ToLower());
            }
            if(person == null)
            {
                ModelState.AddModelError("", "Неверное имя пользователя или пароль");
                return View(viewModel);
            }
            else
            {
                var user = await _userRepository.FirstOrDefaultAsync(user => user.Login.ToLower() == viewModel.LoginOrEmail.ToLower() || user.Email.ToLower() == viewModel.LoginOrEmail.ToLower());
                if(!user.IsEmailConfirmed)
                {
                    ModelState.AddModelError("", "Подтвердите аккаунт с помощью сообщения на почте");
                }
                string inputHash;
                using (var shaAlg = Sha3.Sha3256())
                {
                    string inputPassHash = Convert.ToBase64String(shaAlg.ComputeHash(Encoding.UTF8.GetBytes(viewModel.Password)));
                    inputHash = Convert.ToBase64String(shaAlg.ComputeHash(Encoding.UTF8.GetBytes(inputPassHash + user.Salt)));
                }
                if (inputHash.Equals(user.Password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Login),
                        new Claim(ClaimTypes.Role, user.UserRole.ToString())
                    };
                    ClaimsIdentity identity = new ClaimsIdentity(claims, "Cookies");
                    await _context.HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity));
                    return Redirect(viewModel.ReturnUrl ?? "/");
                }
                else
                {
                    ModelState.AddModelError("", "Неверное имя пользователя или пароль");
                    return View(viewModel);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var route = _context.HttpContext.Request.Headers["Referer"].ToString();
            return Redirect(route ??" /");
        }
    }
}
