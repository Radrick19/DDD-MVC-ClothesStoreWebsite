using Store.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Store.Mvc.ViewModels.Administration
{
    public class AddAccountViewModel
    {
        [Required(ErrorMessage = "Укажите логин")]
        [StringLength(30, ErrorMessage = "Слишком длинный логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        [StringLength(40, ErrorMessage = "Слишком длинный пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Укажите почту")]
        [EmailAddress(ErrorMessage = "Неверно указана почта")]
        [StringLength(100, ErrorMessage = "Слишком длинная почта")]
        public string Email { get; set; }

        public UserRole Role { get; set; } = UserRole.Customer;
    }
}
