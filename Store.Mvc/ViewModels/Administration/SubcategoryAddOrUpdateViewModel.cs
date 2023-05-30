using Store.Application.Dto.Administration;
using Store.Domain.Models.ProductEntities;

namespace Store.API.ViewModels.Administration
{
    public class SubcategoryAddOrUpdateViewModel : BaseAdmininstrationViewModel
    {
        public SubcategoryDto Subcategory { get; set; }
        public IEnumerable<CategoryDto> Categories { get; set; }
    }
}
