using Store.Application.Dto.Administration;

namespace Store.API.ViewModels.Components
{
    public class MobileDropDownViewModel
    {
        public string WomenCategoryName { get; set; } = "Женщины";
        public string MenCategoryName { get; set; } = "Мужчины";
        public string KidsCategoryName { get; set; } = "Дети";
        public string BabyCategoryName { get; set; } = "Младенцы";

        public IEnumerable<CollectionDto> WomenCollections { get; set; }
        public IEnumerable<CollectionDto> MenCollections { get; set; }
        public IEnumerable<CollectionDto> KidsCollections { get; set; }
        public IEnumerable<CollectionDto> BabyCollections { get; set; }

        public IEnumerable<SubcategoryDto> WomenSubcategories { get; set; }
        public IEnumerable<SubcategoryDto> MenSubcategories { get; set; }
        public IEnumerable<SubcategoryDto> KidsSubcategories { get; set; }
        public IEnumerable<SubcategoryDto> BabySubcategories { get; set; }
    }
}
