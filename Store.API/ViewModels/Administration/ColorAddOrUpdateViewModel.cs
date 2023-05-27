using Store.Application.Dto.Administration;
using Store.Domain.Models.ProductEntities;

namespace Store.API.ViewModels.Administration
{
    public class ColorAddOrUpdateViewModel : BaseAdmininstrationViewModel
    {
        public ColorDto Color { get; set; }
    }
}
