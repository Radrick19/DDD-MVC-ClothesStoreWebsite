using Store.Domain.Models.ProductEntities;
using System.Text.Json;
using Store.Domain.Interfaces;
using Store.Api.Interfaces;
using Store.API.Models.Personal;
using System.Text;
using System.Collections.Generic;

namespace Store.API.Infrastructure
{
    public class CartService : ICartService
    {
        private readonly IRepository<Product> _productsRepository;

        public CartService(IRepository<Product> productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task<IEnumerable<CartItem>> GetCartItems(string jsonCookieCartItems)
        {
            List<CookieCartItem> cookieCartItems = JsonSerializer.Deserialize<List<CookieCartItem>>(jsonCookieCartItems);
            List<CartItem> cartItems = new List<CartItem>();
            IProductRepository productRepository = _productsRepository as IProductRepository;
            foreach (var item in cookieCartItems)
            {
                var product = await productRepository.GetByArticleAsync(item.Article);
                CartItem cartItem = new CartItem
                {
                    Name = product.Name,
                    MainPicture = product.MainPicture,
                    Color = item.Color,
                    Size = item.Size,
                    Price = product.Price,
                    Quantity = item.Quantity,
                    Article = item.Article
                };
                cartItems.Add(cartItem);
            }
            return cartItems;
        }
    }
}
