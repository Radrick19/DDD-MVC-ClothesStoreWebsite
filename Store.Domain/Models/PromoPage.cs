using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Models
{
    public class PromoPage : Entity
    {
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string PictureLink { get; set; }
        public virtual string PromoLink { get; set; }
        public virtual int Order { get; set; }

        protected PromoPage()
        {
            
        }
    }
}
