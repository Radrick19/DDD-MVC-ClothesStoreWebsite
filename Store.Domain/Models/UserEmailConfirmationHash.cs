using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
