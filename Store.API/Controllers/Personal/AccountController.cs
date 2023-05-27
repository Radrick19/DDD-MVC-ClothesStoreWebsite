using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using SHA3.Net;
using Store.API.ViewModels.Account;
using Store.Domain.Enums;
using Store.Domain.Interfaces;
using Store.Domain.Models;
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

        public AccountController(IRepository<User> userRepository, IUnitOfWork unitOfWork, IHttpContextAccessor context)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel viewModel)
        {
            if(_userRepository.GetQuary().Any(user=> user.Login == viewModel.Login)) 
            {
                ModelState.AddModelError("Login", "Данный логин уже занят");
            }
            if (_userRepository.GetQuary().Any(user => user.Email == viewModel.Email))
            {
                ModelState.AddModelError("Email", "Данный email уже занят");
            }
            if (ModelState.IsValid)
            {
                byte[] hash;
                using (var shaAlg = Sha3.Sha3256())
                {
                    hash = shaAlg.ComputeHash(Encoding.UTF8.GetBytes(viewModel.Password));
                }
                var user = new User
                {
                    Login = viewModel.Login,
                    Email = viewModel.Email,
                    Password = hash,
                    Guid = Guid.NewGuid(),
                    UserRole = UserRole.Customer
                };
                await _userRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Login");
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
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }
            User person;
            person = await _userRepository.FirstOrDefaultAsync(user => user.Login == viewModel.LoginOrEmail);
            if(person == null)
            {
                person = await _userRepository.FirstOrDefaultAsync(user => user.Email == viewModel.LoginOrEmail);
            }
            if(person == null)
            {
                ModelState.AddModelError("", "Неверное имя пользователя или пароль");
                return View(viewModel);
            }
            else
            {
                var user = await _userRepository.FirstOrDefaultAsync(user => user.Login == viewModel.LoginOrEmail || user.Email == viewModel.LoginOrEmail);
                byte[] hash;
                using (var shaAlg = Sha3.Sha3256())
                {
                    hash = shaAlg.ComputeHash(Encoding.UTF8.GetBytes(viewModel.Password));
                }
                if (user.Password.SequenceEqual(hash))
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
