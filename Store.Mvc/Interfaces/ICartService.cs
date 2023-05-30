using Store.API.Models.Personal;

namespace Store.Api.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<CartItem>> GetCartItems(string jsonCookieCartItems);
    }
}
