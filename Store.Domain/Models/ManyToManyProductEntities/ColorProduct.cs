using Store.Domain.Models.ProductEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Domain.Models.ManyToManyProductEntities
{
    public class ColorProduct : Entity
    {
        [NotMapped]
        public override int Id { get => base.Id; set => base.Id = value; }

        public virtual int ColorId { get; set; }

        [ForeignKey("ColorId")]
        public virtual Color Color { get; set; }

        public virtual int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public ColorProduct()
        {
            
        }

        public ColorProduct(Product product, int colorId)
        {
            Product = product;
            ColorId = colorId;
        }
    }
}
