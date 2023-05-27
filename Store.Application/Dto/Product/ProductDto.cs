using Store.Domain.Models.ManyToManyProductEntities;
using Store.Domain.Models.ProductEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Store.Application.Dto.Administration;

namespace Store.Application.Dto.Product
{
    public class ProductDto
    {
        public int Id { get; set; }

        [DisplayName("Подкатегория")]
        [Required(ErrorMessage = "Необходимо выбрать категорию")]
        public int SubcategoryId { get; set; }

        public SubcategoryDto Subcategory { get; set; }

        public string Article { get; set; }

        public string Name { get; set; }

        [DisplayName("Цена")]
        public int Price { get; set; }

        public string MainPicture { get; set; }

        public string[] AdditionalPictures { get; set; }

        public ICollection<CollectionProduct> Collections { get; set; } = new List<CollectionProduct>();

        public ICollection<ColorProduct> Colors { get; set; } = new List<ColorProduct>();

        public ICollection<ProductSize> Sizes { get; set; } = new List<ProductSize>();

        [DisplayName("Материал")]
        [StringLength(150, ErrorMessage = "Поле не может привышать 150 символов")]
        public string Material { get; set; }

        [DisplayName("Инструкция по уходу")]
        [StringLength(150, ErrorMessage = "Поле не может привышать 150 символов")]
        public string CareInstuctions { get; set; }

        [DisplayName("Описание")]
        [StringLength(350, ErrorMessage = "Поле не может привышать 350 символов")]
        public string Description { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
