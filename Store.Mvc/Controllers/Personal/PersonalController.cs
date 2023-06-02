using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Store.Api.Interfaces;
using Store.API.Models.Personal;
using Store.Application.Dto.Administration;
using Store.Domain.Interfaces;
using Store.Domain.Models.ProductEntities;
using System.Text.Json;

namespace Store.API.Controllers.Personal
{
    public class PersonalController : Controller
    {
        private const string cartCookieKey = "UserCart_Cookies";

        private readonly ICartService _cartService;
        private readonly IRepository<Color> _colorRepository;
        private readonly IRepository<Size> _sizeRepository;
        private readonly IMapper _mapper;

        public PersonalController(ICartService cartService, IRepository<Color> colorRepository, IRepository<Size> sizeRepository, IMapper mapper)
        {
            _cartService = cartService;
            _colorRepository = colorRepository;
            _sizeRepository = sizeRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Cart()
        {
            var cookiesValue = Request.Cookies[cartCookieKey];
            ViewBag.CartCost = 0;
            if (!string.IsNullOrEmpty(cookiesValue))
            {
                var items = await _cartService.GetCartItems(cookiesValue);
                ViewBag.CartCost = items.Select(item => item.Price * item.Quantity).Sum();
                return View(items);
            }
            return View();
        }

        public IActionResult AddItemToCart(string article, string colorName, string sizeName)
        {
            var cartItem = new CookieCartItem()
            {
                Article = article,
                Color = _mapper.Map<ColorDto>(_colorRepository.GetQuary().Where(color => color.Name == colorName).FirstOrDefault()),
                Size = _mapper.Map<SizeDto>(_sizeRepository.GetQuary().Where(size => size.Name == sizeName).FirstOrDefault()),
                Quantity = 1
            };
            var cartItems = GetCookieCartItems();
            if (cartItems != null)
            {
                if (cartItems.Any(ci => ci.Article == article && ci.Size.Name == sizeName && ci.Color.Name == colorName))
                    cartItems.Where(ci => ci.Article == article && ci.Size.Name == sizeName && ci.Color.Name == colorName).First().Quantity++;
                else
                    cartItems.Add(cartItem);
            }
            else
            {
                cartItems = new List<CookieCartItem>
                {
                    cartItem
                };
            }
            Response.Cookies.Delete(cartCookieKey);
            CookieOptions cookieOptions = new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(1)
            };
            string serializedCartItems = JsonSerializer.Serialize(cartItems);
            Response.Cookies.Append(cartCookieKey, serializedCartItems, cookieOptions);
            return RedirectToAction("Product", "Catalog", new { productArticle = article, colorName = colorName, sizeName = sizeName});
        }

        public IActionResult DeleteItemFromCart(string article, string colorName, string sizeName)
        {
            var cartItems = GetCookieCartItems();
            var itemToDelete = cartItems.Where(item => item.Article == article && item.Size.Name == sizeName && item.Color.Name == colorName).First();
            cartItems.Remove(itemToDelete);
            Response.Cookies.Delete(cartCookieKey);
            CookieOptions cookieOptions = new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(1)
            };
            string serializedCartItems = JsonSerializer.Serialize(cartItems);
            Response.Cookies.Append(cartCookieKey, serializedCartItems, cookieOptions);
            return RedirectToAction("Cart");
        }

        public IActionResult IncreaseQuantity(string article, string colorName, string sizeName)
        {
            var cartItems = GetCookieCartItems();
            cartItems.Where(ci => ci.Article == article && ci.Size.Name == sizeName && ci.Color.Name == colorName).First().Quantity++;
            Response.Cookies.Delete(cartCookieKey);
            CookieOptions cookieOptions = new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(1)
            };
            string serializedCartItems = JsonSerializer.Serialize(cartItems);
            Response.Cookies.Append(cartCookieKey, serializedCartItems, cookieOptions);
            return RedirectToAction("Cart");
        }

        public IActionResult ReduceQuantity(string article, string colorName, string sizeName)
        {
            var cartItems = GetCookieCartItems();
            if(cartItems.Where(ci => ci.Article == article && ci.Size.Name == sizeName && ci.Color.Name == colorName).First().Quantity == 1)
            {
                var itemToDelete = cartItems.Where(item => item.Article == article && item.Size.Name == sizeName && item.Color.Name == colorName).First();
                cartItems.Remove(itemToDelete);
            }
            else
            {
                cartItems.Where(ci => ci.Article == article && ci.Size.Name == sizeName && ci.Color.Name == colorName).First().Quantity--;
            }
            Response.Cookies.Delete(cartCookieKey);
            CookieOptions cookieOptions = new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(1)
            };
            string serializedCartItems = JsonSerializer.Serialize(cartItems);
            Response.Cookies.Append(cartCookieKey, serializedCartItems, cookieOptions);
            return RedirectToAction("Cart");
        }

        private List<CookieCartItem> GetCookieCartItems()
        {
            var cookiesValue = Request.Cookies[cartCookieKey];
            if (cookiesValue != null)
            {
                var cartItems = JsonSerializer.Deserialize<List<CookieCartItem>>(cookiesValue);
                return cartItems;
            }
            return null;
        }
    }
}
