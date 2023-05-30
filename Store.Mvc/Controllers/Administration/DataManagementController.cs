using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Interfaces;
using Store.Domain.Models.ManyToManyProductEntities;
using Store.Domain.Models.ProductEntities;

namespace Store.API.Controllers.Administration
{
    [Authorize(Roles = "Moderator, Administrator")]
    public class DataManagementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<CollectionProduct> _collectionProductRepository;
        private readonly IRepository<Subcategory> _subcategoriesRepository;

        public DataManagementController(IUnitOfWork unitOfWork, IRepository<CollectionProduct> collectionProductRepository, 
            IRepository<Subcategory> subcategoriesRepository)
        {
            _unitOfWork = unitOfWork;
            _collectionProductRepository = collectionProductRepository;
            _subcategoriesRepository = subcategoriesRepository;
        }

        [HttpGet("datamanagement")]
        public async Task<IActionResult> Index()
        {
            ViewBag.WomenCollections = await _collectionProductRepository.GetQuary()
                .Where(cp => cp.Product.Subcategory.CategoryId == 1)
                .Select(cp => cp.Collection)
                .Distinct()
                .OrderBy(cc => cc.Order)
                .Take(10)
                .ToListAsync();
            ViewBag.MenCollections = await _collectionProductRepository.GetQuary()
                .Where(cp => cp.Product.Subcategory.CategoryId == 2)
                .Select(cp => cp.Collection)
                .Distinct()
                .OrderBy(cc => cc.Order)
                .Take(10)
                .ToListAsync();
            ViewBag.KidsCollections = await _collectionProductRepository.GetQuary()
                .Where(cp => cp.Product.Subcategory.CategoryId == 3)
                .Select(cp => cp.Collection)
                .Distinct()
                .OrderBy(cc => cc.Order)
                .Take(10)
                .ToListAsync();
            ViewBag.BabyCollections = await _collectionProductRepository.GetQuary()
                .Where(cp => cp.Product.Subcategory.CategoryId == 4)
                .Select(cp => cp.Collection)
                .Distinct()
                .OrderBy(cc => cc.Order)
                .Take(10)
                .ToListAsync();
            ViewBag.WomenSubcategories = await _subcategoriesRepository.GetQuary()
                .Where(sc => sc.CategoryId == 1)
                .OrderBy(col => col.Order)
                .Take(10)
                .ToListAsync();
            ViewBag.MenSubcategories = await _subcategoriesRepository.GetQuary()
                .Where(sc => sc.CategoryId == 2)
                .OrderBy(col => col.Order)
                .Take(10)
                .ToListAsync();
            ViewBag.KidsSubcategories = await _subcategoriesRepository.GetQuary()
                .Where(sc => sc.CategoryId == 3)
                .OrderBy(col => col.Order)
                .Take(10)
                .ToListAsync();
            ViewBag.BabySubcategories = await _subcategoriesRepository.GetQuary()
                .Where(sc => sc.CategoryId == 4)
                .OrderBy(col => col.Order)
                .Take(10)
                .ToListAsync();
            return View();
        }
    }
}
