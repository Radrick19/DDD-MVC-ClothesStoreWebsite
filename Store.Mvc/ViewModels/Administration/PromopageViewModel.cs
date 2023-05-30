using Store.Application.Dto.Administration;

namespace Store.API.ViewModels.Administration
{
    public class PromoPageAddOrUpdateViewModel : BaseAdmininstrationViewModel
    {
        public PromoPageDto PromoPage { get; set; }
        public IFormFile Image { get; set; }
    }
}
