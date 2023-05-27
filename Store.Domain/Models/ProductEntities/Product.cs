using Store.Domain.Models.ManyToManyProductEntities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Domain.Models.ProductEntities
{
    public class Product : Entity
    {
        public virtual int SubcategoryId { get; set; }

        [ForeignKey("SubcategoryId")]
        public virtual Subcategory Subcategory { get; set; }

        public virtual string Article { get; set; }

        public virtual string Name { get; set; }

        public virtual int Price { get; set; }

        public virtual string MainPicture { get; set; }

        public virtual string[] AdditionalPictures { get; set; }

        public virtual ICollection<CollectionProduct> Collections { get; set; }

        public virtual ICollection<ColorProduct> Colors { get; set; }

        public virtual ICollection<ProductSize> Sizes { get; set; }

        public virtual string Material { get; set; }

        public virtual string CareInstuctions { get; set; }

        public virtual string Description { get; set; }

        public virtual DateTime CreationTime { get; set; }

        public virtual int CountOfTrasitions { get; set; }

        protected Product()
        {
            
        }
    }
}
