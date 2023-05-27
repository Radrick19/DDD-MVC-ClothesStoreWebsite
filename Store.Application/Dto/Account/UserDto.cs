using Store.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Dto.Account
{
    public class UserDto
    {
        public Guid Guid { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public Byte[] Password { get; set; }
        public UserRole UserRole { get; set; }
    }
}
