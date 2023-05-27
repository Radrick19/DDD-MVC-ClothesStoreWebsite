using Microsoft.AspNetCore.Mvc;
using Store.Api.Interfaces;
using Store.API.Infrastructure;
using Store.API.ViewModels.Components;
using Store.Domain.Enums;
using System;
using System.Security.Claims;

namespace Store.API.Components
{
    public class MainMenuActions : ViewComponent
    {
        private const string cartCookieKey = "UserCart_Cookies";

        private readonly ICartService _cartService;

        private readonly IHttpContextAccessor _context;

        public MainMenuActions(IHttpContextAccessor context, ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModel = new MainMenuActionsViewModel();
            var user = _context.HttpContext.User;
            if (user != null && user.Identity.IsAuthenticated)
            {
                string role = user.FindFirst(ClaimTypes.Role).Value;
                Enum.TryParse(role, out UserRole userRole);
                viewModel.UserRole = userRole;
                viewModel.Username = user.Identity.Name;
            }

            var cookiesValue = Request.Cookies[cartCookieKey];
            if (!string.IsNullOrEmpty(cookiesValue))
            {
                var items = await _cartService.GetCartItems(cookiesValue);
                viewModel.ItemsInCart = items.Sum(item => item.Quantity);
            }
            return View(viewModel);
        }
    }
}