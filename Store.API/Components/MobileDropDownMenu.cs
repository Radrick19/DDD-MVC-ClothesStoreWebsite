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
    public class MobileDropDownMenu : ViewComponent
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<CollectionProduct> _collectionProductRepository;
        private readonly IMapper _mapper;

        public MobileDropDownMenu(IRepository<Product> productRepository,
            IRepository<CollectionProduct> collectionProductRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _collectionProductRepository = collectionProductRepository;
            _mapper = mapper;
        }

        public IViewComponentResult Invoke()
        {
            MobileDropDownViewModel viewModel = new MobileDropDownViewModel()
            {
                WomenCollections = FillCollection(1),
                MenCollections = FillCollection(2),
                KidsCollections = FillCollection(3),
                BabyCollections = FillCollection(4),
                WomenSubcategories = FillSubcategory(1),
                MenSubcategories = FillSubcategory(2),
                KidsSubcategories = FillSubcategory(3),
                BabySubcategories = FillSubcategory(4),
            };
            return View(viewModel);
        }

        private IEnumerable<CollectionDto> FillCollection(int categoryId)
        {
            return _collectionProductRepository
                .GetQuary()
                .Where(cp => cp.Product.Subcategory.CategoryId == categoryId)
                .Select(cp => _mapper.Map<CollectionDto>(cp.Collection))
                .Distinct();
        }

        private IEnumerable<SubcategoryDto> FillSubcategory(int categoryId)
        {
            return _productRepository
                .GetQuary()
                .Where(prod => prod.Subcategory.CategoryId == categoryId)
                .Select(cp => _mapper.Map<SubcategoryDto>(cp.Subcategory))
                .Distinct();
        }
    }
}
