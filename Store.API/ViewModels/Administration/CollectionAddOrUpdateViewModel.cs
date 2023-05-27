using Store.Application.Dto.Administration;
using Store.Domain.Models.ProductEntities;

namespace Store.API.ViewModels.Administration
{
    public class CollectionAddOrUpdateViewModel : BaseAdmininstrationViewModel
    {
        public CollectionDto Collection { get; set; }
    }
}
