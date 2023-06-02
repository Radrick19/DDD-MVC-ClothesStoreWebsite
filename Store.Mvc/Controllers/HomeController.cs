using Microsoft.AspNetCore.Mvc;
using Store.API.ViewModels;
using Store.Domain.Interfaces;
using Store.Domain.Models;

namespace Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRepository<PromoPage> _promoPageRepository;

        public HomeController(IRepository<PromoPage> promoPageRepository, IWebHostEnvironment webHostEnvironment)
        {
            _promoPageRepository = promoPageRepository;
            _webHostEnvironment = webHostEnvironment;
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
