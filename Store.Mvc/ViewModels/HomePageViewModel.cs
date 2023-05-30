using Store.Domain.Models;

namespace Store.API.ViewModels
{
    public class HomePageViewModel
    {
        public IEnumerable<PromoPage> PromoPages { get; set; }
    }
}
