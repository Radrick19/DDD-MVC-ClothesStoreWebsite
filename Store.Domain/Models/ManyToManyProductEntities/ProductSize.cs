using Store.Domain.Models.ProductEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Domain.Models.ManyToManyProductEntities
{
    public class ProductSize : Entity
    {
        [NotMapped]
        public override int Id { get => base.Id; set => base.Id = value; }

        public virtual int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public virtual int SizeId { get; set; }

        [ForeignKey("SizeId")]
        public virtual Size Size { get; set; }

        public ProductSize()
        {
            
        }

        public ProductSize(Product product, int sizeId)
        {
            Product = product;
            SizeId = sizeId;
        }
    }
}
