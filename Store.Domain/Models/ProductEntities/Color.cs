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

        public Color(int id, string name, string hex, int order)
        {
            Id = id;
            Name = name;
            Hex = hex;
            Order = order;
        }

        protected Color()
        {
            
        }
    }
}
