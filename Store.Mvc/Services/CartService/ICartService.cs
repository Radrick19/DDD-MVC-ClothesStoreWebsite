using Store.Mvc.Services.CartService.Models;

namespace Store.Mvc.Services.CartService
{
    public interface ICartService
    {
        Task<IEnumerable<CartItem>> GetCartItems(string jsonCookieCartItems);
    }
}
