using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Dto.Administration;
using Store.Domain.Interfaces;
using Store.Domain.Models.ProductEntities;
using Store.Mvc.ViewModels.Menu;

namespace Store.Mvc.Controllers
{
    public class Menu : Controller
    {
        private readonly IRepository<Category> _categoriesRepository;
        private readonly IRepository<ClothingCollection> _collectionRepository;
        private readonly IRepository<Subcategory> _subcategoryRepository;
        private readonly IMapper _mapper;

        public Menu(IRepository<ClothingCollection> collectionRepository, IRepository<Category> categoriesRepository,
            IRepository<Subcategory> subcategoryRepository, IMapper mapper)
        {
            _collectionRepository = collectionRepository;
            _categoriesRepository = categoriesRepository;
            _subcategoryRepository = subcategoryRepository;
            _mapper = mapper;
        }

        public IActionResult GetMobileMenu()
        {
            return PartialView();
        }

        public IActionResult GetMenu()
        {
            return PartialView();
        }

        [HttpGet("getsubcategories/{categoryId}/{isForMobile:bool}")]
        public async Task<IActionResult> GetSubcategories(int categoryId, bool isForMobile)
        {
            var category = await _categoriesRepository.FirstOrDefaultAsync(c => c.Id == categoryId);
            var viewModel = new SubcategoriesMenuViewModel()
            {
                CategoryName = category.Name,
                Subcategories = _subcategoryRepository
                .GetQuary()
                .Where(sc => sc.CategoryId == categoryId && sc.Products.Any())
                .Select(sc => _mapper.Map<SubcategoryDto>(sc)),
                IsForMobile = isForMobile
            };
            return PartialView("MenuLinks", viewModel);
        }

        [HttpGet("getcollections/{categoryId}/{isForMobile:bool}")]
        public async Task<IActionResult> GetCollections(int categoryId, bool isForMobile)
        {
            var category = await _categoriesRepository.FirstOrDefaultAsync(c => c.Id == categoryId);
            var viewModel = new CollectionsMenuViewModel()
            {
                CategoryName = category.Name,
                Collections = _collectionRepository
                .GetQuary()
                .Where(c => c.Products.Any(prod => prod.Product.Subcategory.CategoryId == categoryId))
                .Select(col => _mapper.Map<CollectionDto>(col)),
                IsForMobile = isForMobile
            };
            return PartialView("MenuLinks", viewModel);
        }

        public Task<JsonResult> GetSubcatgories()
        {
            throw new NotImplementedException();
        }
    }
}
