using Store.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Store.API.ViewModels.Account
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "Укажите логин")]
        [StringLength(30, ErrorMessage = "Слишком длинный логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        [StringLength(40, ErrorMessage = "Слишком длинный пароль")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,}$", ErrorMessage = "Пароль должен быть не менее чем из 8 символов. Должна быть: одна заглавная буква и минимум одна цифра")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "Укажите почту")]
        [EmailAddress(ErrorMessage = "Неверно указана почта")]
        [StringLength(100, ErrorMessage = "Слишком длинная почта")]
        public string Email { get; set; }

        public UserRole Role { get; set; } = UserRole.Customer;

        public string captchaToken { get; set; }
    }
}
