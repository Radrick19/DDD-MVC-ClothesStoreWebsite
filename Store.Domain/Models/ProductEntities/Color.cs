using Store.Domain.Models.ManyToManyProductEntities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Store.Domain.Models.ProductEntities
{
    public class Color : Entity
    {
        public virtual string Name { get; set; }

        public virtual string Hex { get; set; }

        public virtual int Order { get; set; }

        public virtual ICollection<ColorProduct> Products { get; set; }

        protected Color()
        {
            
        }
    }
}
