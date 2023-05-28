using Store.Domain.Enums;

namespace Store.Domain.Models
{
    public class User : Entity
    {
        public virtual Guid Guid { get; set; }
        public virtual string Login { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual string Salt { get; set; }
        public virtual bool IsEmailConfirmed { get; set; }
        public virtual UserRole UserRole { get; set; }
    }
}
