using Store.Application.Dto.Administration;

namespace Store.Mvc.ViewModels.Menu
{
    public class SubcategoriesMenuViewModel
    {
        public IEnumerable<SubcategoryDto> Subcategories { get; set; }
        public string CategoryName { get; set; }
        public bool IsForMobile { get; set; }
    }
}
