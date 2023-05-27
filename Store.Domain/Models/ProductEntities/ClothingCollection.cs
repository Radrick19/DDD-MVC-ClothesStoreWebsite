using Store.Domain.Models.ManyToManyProductEntities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Store.Domain.Models.ProductEntities
{
    public class ClothingCollection : Entity
    {
        public virtual string Name { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual string Description { get; set; }

        public virtual bool IsActual { get; set; } = true;

        public virtual int Order { get; set; }

        public virtual ICollection<CollectionProduct> Products { get; set; }

        protected ClothingCollection()
        {
            
        }
    }
}
