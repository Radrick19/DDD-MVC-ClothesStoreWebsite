using Store.API.Models;
using Store.Application.Dto.Administration;
using Store.Application.Dto.Product;

namespace Store.API.ViewModels.Catalog
{
    public class DetailsCatalogViewModel
    {
        public ProductDetailsDto Product { get; set; }
        public IEnumerable<ProductCatalogDto> SimularProducts { get; set; }
        public ColorDto SelectedColor { get; set; }
        public SizeDto SelectedSize { get; set; }
        public List<BreadcrumbItem> Breadcrumbs { get; set; }
    }
}
