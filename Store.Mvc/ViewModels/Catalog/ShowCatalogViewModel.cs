using Store.API.Models;
using Store.Application.Infrastructure;
using Store.Domain.Models.ProductEntities;

namespace Store.API.ViewModels.Catalog
{
    public class ShowCatalogViewModel
    {
        public string SectionName { get; set; }
        public string SectionDescription { get; set; }
        public List<BreadcrumbItem> Breadcrumbs { get; set; }
        public int? CategoryId { get; set; }
        public int? SubcategoryId { get; set; }
        public int? CollectionId { get; set; }
        public string SearchText { get; set; }
    }
}
