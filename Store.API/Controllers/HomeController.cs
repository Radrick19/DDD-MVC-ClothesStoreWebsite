using Microsoft.AspNetCore.Mvc;
using Store.API.ViewModels;
using Store.Domain.Interfaces;
using Store.Domain.Models;

namespace Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<PromoPage> _promoPageRepository;

        public HomeController(IRepository<PromoPage> promoPageRepository)
        {
            _promoPageRepository = promoPageRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            HomePageViewModel viewModel = new HomePageViewModel()
            {
                 PromoPages = _promoPageRepository.GetQuary()
                .OrderBy(pp=> pp.Order)
                .ThenBy(pp=> pp.Title)
            };
            return View(viewModel);
        }
    }
}
