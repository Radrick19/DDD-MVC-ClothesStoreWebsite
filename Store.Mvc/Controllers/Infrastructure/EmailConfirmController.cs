using Microsoft.AspNetCore.Mvc;
using Store.Domain.Interfaces;
using Store.Domain.Models;
using System.Drawing.Printing;

namespace Store.Mvc.Controllers.Infrastructure
{
    public class EmailConfirmController : Controller
    {
        private readonly IRepository<UserEmailConfirmationHash> _userEmailConfirmHashRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmailConfirmController(IRepository<UserEmailConfirmationHash> userEmailConfirmHash, IRepository<User> userRepository, IUnitOfWork unitOfWork)
        {
            _userEmailConfirmHashRepository = userEmailConfirmHash;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("emailconfirm/{hash}")]
        public async Task<IActionResult> Index(string hash)
        {
            var userAndEmailHash = await _userEmailConfirmHashRepository.FirstOrDefaultAsync(ue=> ue.ConfirmationHash == hash);
            if(userAndEmailHash != null)
            {
                User user = userAndEmailHash.User;
                user.IsEmailConfirmed = true;
                _userRepository.Update(user);
                _userEmailConfirmHashRepository.Delete(userAndEmailHash);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index", "Message", new { text = "Аккаунт успешно подтверждён" });
            }
            return BadRequest();
        }
    }
}
