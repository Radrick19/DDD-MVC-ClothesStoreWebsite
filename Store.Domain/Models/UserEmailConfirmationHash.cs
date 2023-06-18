using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Domain.Models
{
    public class UserEmailConfirmationHash : Entity
    {
        public virtual int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual string ConfirmationHash { get; set; }

        public virtual DateTime CreationDate { get; set; }
    }
}
