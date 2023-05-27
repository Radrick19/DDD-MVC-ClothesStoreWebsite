using Store.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Models
{
    public class User : Entity
    {
        public virtual Guid Guid { get; set; }
        public virtual string Login { get; set; }
        public virtual string Email { get; set; }
        public virtual Byte[] Password { get; set; }
        public virtual UserRole UserRole { get; set; }
    }
}
