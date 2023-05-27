using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHA3.Net;
using Store.API.ViewModels.Account;
using Store.Domain.Enums;
using Store.Domain.Interfaces;
using Store.Domain.Models;
using System.Data;
using System.Text;

namespace Store.API.Controllers.Administration
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IRepository<User> userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetQuary().Where(user => user.UserRole != UserRole.Customer).ToListAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(RegistrationViewModel viewModel, UserRole role)
        {
            if (_userRepository.GetQuary().Any(user => user.Login == viewModel.Login))
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
                    UserRole = viewModel.Role
                };
                await _userRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _userRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
