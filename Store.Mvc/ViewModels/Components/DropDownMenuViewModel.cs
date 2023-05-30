using Store.Application.Dto.Administration;

namespace Store.API.ViewModels.Components
{
    public class DropDownMenuViewModel
    {
        public CategoryDto Category { get; set; }
        public IEnumerable<CollectionDto> Collections { get; set; }
        public IEnumerable<SubcategoryDto> Subcategories { get; set; }
    }
}
