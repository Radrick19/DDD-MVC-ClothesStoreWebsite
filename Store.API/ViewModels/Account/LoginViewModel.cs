using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Store.API.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Укажите логин")]
        [StringLength(30, ErrorMessage = "Слишком длинный логин")]
        public string LoginOrEmail { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        [StringLength(40, ErrorMessage = "Слишком длинный пароль")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public string captchaToken { get; set; }
    }
}
