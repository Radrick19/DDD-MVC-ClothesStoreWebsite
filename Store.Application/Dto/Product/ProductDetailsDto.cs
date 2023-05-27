using Store.Domain.Models.ManyToManyProductEntities;

namespace Store.Application.Dto.Product
{
    public class ProductDetailsDto
    {
        public string Article { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string MainPicture { get; set; }

        public string[] AdditionalPictures { get; set; }

        public bool CanReturn { get; set; }

        public ICollection<ColorProduct> Colors { get; set; }

        public ICollection<ProductSize> Sizes { get; set; }

        public string Material { get; set; }

        public string CareInstuctions { get; set; }

        public string Description { get; set; }
    }
}
