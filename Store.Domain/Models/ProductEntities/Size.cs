using Store.Domain.Models.ManyToManyProductEntities;

namespace Store.Domain.Models.ProductEntities
{
    public class Size : Entity
    {
        public virtual string Name { get; set; }

        public virtual int Order { get; set; }

        public virtual ICollection<ProductSize> Products { get; set; }

        public Size(int id, string name, int order)
        {
            Id = id;
            Name = name;
            Order = order;
        }

        protected Size()
        {
            
        }
    }
}
