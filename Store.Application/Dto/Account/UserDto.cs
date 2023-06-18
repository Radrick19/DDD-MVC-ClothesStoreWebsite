using Store.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Store.Application.Dto.Account
{
    public class UserDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public UserRole UserRole { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
