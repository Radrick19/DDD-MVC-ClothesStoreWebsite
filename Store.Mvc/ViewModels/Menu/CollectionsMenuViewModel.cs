using Store.Application.Dto.Administration;
using Store.Domain.Models.ProductEntities;

namespace Store.Mvc.ViewModels.Menu
{
    public class CollectionsMenuViewModel
    {
        public IEnumerable<CollectionDto> Collections { get; set; }
        public string CategoryName { get; set; }
        public bool IsForMobile { get; set; }
    }
}
