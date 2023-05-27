using Microsoft.Build.Framework;
using Store.Application.Dto.Administration;
using Store.Application.Dto.Product;
using Store.Domain.Models.ProductEntities;
using System.ComponentModel;

namespace Store.API.ViewModels.Administration
{
    public class ProductAddOrUpdateViewModel : BaseAdmininstrationViewModel
    {
        public ProductDto Product { get; set; }
        [DisplayName("Главное изображение")]
        public IFormFile MainImage { get; set; }
        [DisplayName("Дополнительные изображения")]
        public IEnumerable<IFormFile> AdditionalImages { get; set; }
        [DisplayName("Размеры")]
        public int[] SelectedSizeIds { get; set; }
        [DisplayName("Цвета")]
        public int[] SelectedColorIds { get; set; }
        [DisplayName("Коллекции")]
        public int[] SelectedCollectionIds { get; set; }

        public IEnumerable<SizeDto> Sizes { get; set; }
        public IEnumerable<ColorDto> Colors { get; set; }
        public IEnumerable<CollectionDto> Collections { get; set; }
    }
}
