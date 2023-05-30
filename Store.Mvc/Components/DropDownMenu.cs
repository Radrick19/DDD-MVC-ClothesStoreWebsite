using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Store.API.ViewModels.Components;
using Store.Application.Dto.Administration;
using Store.Domain.DatabaseRepositories.Postgre;
using Store.Domain.Interfaces;
using Store.Domain.Models.ManyToManyProductEntities;
using Store.Domain.Models.ProductEntities;

namespace Store.API.Components
{
    public class DropDownMenu : ViewComponent
    {
        private readonly IRepository<Category> _categoriesRepository;
        private readonly IRepository<Subcategory> _subcategoriesRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<CollectionProduct> _collectionProductRepository;
        private readonly IMapper _mapper;

        public DropDownMenu(IRepository<Category> categoryRepository, IRepository<Subcategory> subcategoryRepository, 
            IRepository<CollectionProduct> collectionProductRepository, IRepository<Product> productRepository
            ,IMapper mapper)
        {
            _categoriesRepository = categoryRepository;
            _subcategoriesRepository = subcategoryRepository;
            _collectionProductRepository = collectionProductRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int category)
        {
            DropDownMenuViewModel viewModel = new DropDownMenuViewModel()
            {
                Category = _mapper.Map<CategoryDto>(await _categoriesRepository.GetAsync(category)),
                Subcategories = _productRepository
                    .GetQuary()
                    .Where(prod => prod.Subcategory.CategoryId == category)
                    .Select(prod => prod.Subcategory)
                    .Distinct()
                    .Take(10)
                    .OrderBy(sc => sc.Order)
                    .ThenBy(sc => sc.Name)
                    .Select(sc => _mapper.Map<SubcategoryDto>(sc)),

                Collections = _collectionProductRepository
                    .GetQuary()
                    .Where(cp => cp.Product.Subcategory.CategoryId == category)
                    .Select(cp => cp.Collection)
                    .Distinct()
                    .OrderBy(cc => cc.Order)
                    .Take(10)
                    .Select(col=> _mapper.Map<CollectionDto>(col))
            };
            return View(viewModel);
        }
    }
}
