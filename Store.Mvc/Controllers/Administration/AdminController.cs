using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHA3.Net;
using Store.API.ViewModels.Account;
using Store.Application.Dto.Account;
using Store.Domain.Enums;
using Store.Domain.Interfaces;
using Store.Domain.Models;
using Store.Mvc.ViewModels.Administration;
using System.Data;
using System.Text;

namespace Store.API.Controllers.Administration
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdminController(IRepository<User> userRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
        public async Task<IActionResult> Add(AddAccountViewModel viewModel)
        {
            if (_userRepository.GetQuary().Any(user => user.Login.ToLower() == viewModel.Login.ToLower()))
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
                var user = new UserDto
                {
                    Login = viewModel.Login,
                    Email = viewModel.Email,
                    Password = hash,
                    Guid = Guid.NewGuid(),
                    Salt = salt,
                    UserRole = viewModel.Role,
                    IsEmailConfirmed = true,
                    RegistrationDate = DateTime.UtcNow
                };
                await _userRepository.AddAsync(_mapper.Map<User>(user));
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
