using Store.Domain.Models.ProductEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Domain.Models.ManyToManyProductEntities
{
    public class CollectionProduct : Entity
    {
        [NotMapped]
        public override int Id { get => base.Id; set => base.Id = value; }

        public virtual int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public virtual int CollectionId { get; set; }

        [ForeignKey("CollectionId")]
        public virtual ClothingCollection Collection { get; set; }

        public CollectionProduct()
        {
            
        }

        public CollectionProduct(Product product, int collectionId)
        {
            Product = product;
            CollectionId = collectionId;
        }
    }
}
