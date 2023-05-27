using Store.Domain.Models.ManyToManyProductEntities;

namespace Store.Domain.Models.ProductEntities
{
    public class Size : Entity
    {
        public virtual string Name { get; set; }

        public virtual int Order { get; set; }

        public virtual ICollection<ProductSize> Products { get; set; }

        protected Size()
        {
            
        }
    }
}
