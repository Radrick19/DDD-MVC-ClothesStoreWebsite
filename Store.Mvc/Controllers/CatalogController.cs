using AutoMapper;
using Korzh.EasyQuery.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.API.Models;
using Store.API.ViewModels.Catalog;
using Store.Application.Dto.Administration;
using Store.Application.Dto.Product;
using Store.Application.Enums;
using Store.Application.Services.ProductPopularityService;
using Store.Domain.Infrastructure;
using Store.Domain.Interfaces;
using Store.Domain.Models.ProductEntities;
using System.Net;
using System.Runtime.CompilerServices;

namespace Store.Controllers
{
    public class CatalogController : Controller
    {
        private readonly int _pageSize;
        private readonly IRepository<Product> _productsRepository;
        private readonly IRepository<ClothingCollection> _collectionRepository;
        private readonly IRepository<Subcategory> _subcategoryRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Color> _colorRepository;
        private readonly IRepository<Size> _sizesRepository;
        private readonly IMapper _mapper;
        private readonly IProductPopularityService _popularityService;
        private readonly IConfiguration _configuration;

        public CatalogController(IUnitOfWork unitOfWork, IRepository<Product> productsRepository, IRepository<ClothingCollection> collectionRepository, IRepository<Subcategory> subcategoryRepository,
            IRepository<Category> categoryRepository, IRepository<Color> colorRepository, IRepository<Size> sizesRepository, IMapper mapper, IProductPopularityService popularityService, IConfiguration configuration)
        {
            _productsRepository = productsRepository;
            _collectionRepository = collectionRepository;
            _subcategoryRepository = subcategoryRepository;
            _categoryRepository = categoryRepository;
            _colorRepository = colorRepository;
            _sizesRepository = sizesRepository;
            _mapper = mapper;
            _popularityService = popularityService;
            _configuration = configuration;
            _pageSize = Convert.ToInt32(_configuration.GetSection("CatalogSettings")["ItemsPerPage"]);
        }

        [HttpGet("catalog/paginationline/{categoryId:int}/{subcategoryId:int}/{collectionId:int}/{searchText}/{totalPage:int}")]
        [HttpGet("catalog/paginationline/{categoryId:int}/{subcategoryId:int}/{collectionId:int}/{totalPage:int}")]
        public async Task<IActionResult> PaginationLine(int categoryId, int subcategoryId, int collectionId, string searchText, int totalPage)
        {
            var model = new PaginationLineViewModel()
            {
                CountOfPages = (int)Math.Ceiling(await GetFilteredProducts(categoryId, subcategoryId, collectionId, searchText).CountAsync() / (double)_pageSize),
                TotalPage = totalPage
            };
            if (model.CountOfPages == 0)
            {
                return PartialView();
            }
            return PartialView(model);
        }

        [HttpGet("catalog/itemscount/{categoryId:int}/{subcategoryId:int}/{collectionId:int}/{searchText}")]
        [HttpGet("catalog/itemscount/{categoryId:int}/{subcategoryId:int}/{collectionId:int}")]
        public async Task<IActionResult> ItemsCount(int categoryId, int subcategoryId, int collectionId, string searchText)
        {
            int itemsCount = await GetFilteredProducts(categoryId, subcategoryId, collectionId, searchText).CountAsync();
            return PartialView(itemsCount);
        }

        [HttpGet("catalog/productitems/{categoryId:int}/{subcategoryId:int}/{collectionId:int}/{searchText}/{totalPage:int}/{sortType}")]
        [HttpGet("catalog/productitems/{categoryId:int}/{subcategoryId:int}/{collectionId:int}/{totalPage:int}/{sortType}")]
        public async Task<IActionResult> ProductItems(int categoryId, int subcategoryId, int collectionId, string searchText, int totalPage, int sortType)
        {
            SortTypeEnum totalSortType = (SortTypeEnum)sortType;

            var products = await GetFilteredProducts(categoryId, subcategoryId, collectionId, searchText, totalSortType)
                .Skip((totalPage * _pageSize) - _pageSize)
                .Take(_pageSize)
                .Select(prod => _mapper.Map<ProductCatalogDto>(prod))
                .ToListAsync();

            return PartialView(products);
        }

        [HttpPost]
        public IActionResult Search(ShowCatalogViewModel model)
        {
            string encodedSearchText = WebUtility.UrlEncode(model.SearchText);
            return Redirect($"/catalog/search/{encodedSearchText}");
        }

        [HttpGet("catalog/search/{encodedSearchText}")]
        public IActionResult Search(string encodedSearchText)
        {
            string searchText = WebUtility.UrlDecode(encodedSearchText);
            var model = new ShowCatalogViewModel
            {
                CategoryId = 0,
                SubcategoryId = 0,
                CollectionId = 0,
                SearchText = searchText
            };
            return View("Index", model);
        }


        [HttpGet("catalog/{categoryName}")]
        [HttpGet("catalog/{categoryName}/{subcategoryName}")]
        [HttpGet("catalog/collection/{categoryName}/{collectionName}")]
        public async Task<IActionResult> Index(string categoryName, string subcategoryName, string collectionName)
        {
            string sectionName = string.Empty;
            string sectionDescription = string.Empty;

            var breadCrumbs = new List<BreadcrumbItem>();

            var viewModel = new ShowCatalogViewModel
            {
                CategoryId = 0,
                SubcategoryId = 0,
                CollectionId = 0,
            };

            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                var category = await _categoryRepository.FirstOrDefaultAsync(c => c.Name == categoryName);
                sectionName = category.DisplayName;
                sectionDescription = category.Description;
                breadCrumbs.Add(new BreadcrumbItem("Главная", isActive: true, $"/home"));
                breadCrumbs.Add(new BreadcrumbItem(category.DisplayName, isActive: false, $"/catalog/{category.Name}"));
                viewModel.CategoryId = category.Id;
                if (!string.IsNullOrWhiteSpace(collectionName))
                {
                    var collection = await _collectionRepository.FirstOrDefaultAsync(c => c.Name == collectionName);
                    sectionName = collection.DisplayName;
                    sectionDescription = collection.Description;
                    breadCrumbs.Clear();
                    breadCrumbs.Add(new BreadcrumbItem(category.DisplayName, isActive: true, $"/catalog/{category.Name}"));
                    breadCrumbs.Add(new BreadcrumbItem(collection.DisplayName, isActive: false));
                    viewModel.CollectionId = collection.Id;
                }
                if (!string.IsNullOrWhiteSpace(subcategoryName))
                {
                    var subcategory = await _subcategoryRepository.FirstOrDefaultAsync(s => s.Name == subcategoryName && s.CategoryId == category.Id);
                    sectionName = subcategory.DisplayName;
                    sectionDescription = subcategory.Description;
                    breadCrumbs.Clear();
                    breadCrumbs.Add(new BreadcrumbItem(category.DisplayName, isActive: true, $"/catalog/{category.Name}"));
                    breadCrumbs.Add(new BreadcrumbItem(subcategory.DisplayName, isActive: false));
                    viewModel.SubcategoryId = subcategory.Id;
                }
            }

            viewModel.SectionName = sectionName;
            viewModel.SectionDescription = sectionDescription;
            viewModel.Breadcrumbs = breadCrumbs;

            return View(viewModel);
        }

        [HttpGet("catalog/details/{productArticle}")]
        public async Task<IActionResult> Details(string productArticle)
        {
            Product product = await (_productsRepository as IProductRepository).GetByArticleAsync(productArticle);
            await _popularityService.IncreaseProductPopularityAsync(productArticle);
            DetailsCatalogViewModel detailsCatalogViewModoel = new DetailsCatalogViewModel()
            {
                Product = _mapper.Map<ProductDetailsDto>(product),
                SelectedColor = _mapper.Map<ColorDto>(product.Colors.Select(cp => cp.Color).First()),
                SelectedSize = _mapper.Map<SizeDto>(product.Sizes.Select(ss => ss.Size).First()),
                SimularProducts = await _productsRepository
                .GetQuary()
                .Where(prod => prod.Subcategory.CategoryId == product.Subcategory.CategoryId && prod.Article != product.Article)
                .OrderBy(prod => prod.Name)
                .ThenBy(prod => prod.Price)
                .Take(4)
                .Select(prod => _mapper.Map<ProductCatalogDto>(prod))
                .ToListAsync(),
                Breadcrumbs = new List<BreadcrumbItem>()
                {
                    new BreadcrumbItem(product.Subcategory.Category.DisplayName, isActive:true, $"/catalog/{product.Subcategory.Category.Name}"),
                    new BreadcrumbItem(product.Subcategory.DisplayName, isActive:true, $"/catalog/{product.Subcategory.Category.Name}/{product.Subcategory.Name}"),
                    new BreadcrumbItem(product.Name, isActive:false),
                }
            };
            return View(detailsCatalogViewModoel);
        }

        [HttpGet("catalog/product/{productArticle}/{colorName}/{sizeName?}")]
        public async Task<IActionResult> Product(string productArticle, string colorName, string sizeName = null)
        {
            Product product = await (_productsRepository as IProductRepository).GetByArticleAsync(productArticle);
            Color color = await _colorRepository.FirstOrDefaultAsync(color => color.Name == colorName);
            Size size;
            if (sizeName == null)
            {
                size = product.Sizes.Select(ps => ps.Size).FirstOrDefault();
            }
            else
            {
                size = await _sizesRepository.FirstOrDefaultAsync(size => size.Name == sizeName);
            }
            DetailsCatalogViewModel detailsCatalogViewModoel = new DetailsCatalogViewModel()
            {
                Product = _mapper.Map<ProductDetailsDto>(product),
                SelectedColor = _mapper.Map<ColorDto>(color),
                SelectedSize = _mapper.Map<SizeDto>(size),
                SimularProducts = await _productsRepository
                .GetQuary()
                .Where(prod => prod.Subcategory.CategoryId == product.Subcategory.CategoryId && prod.Article != product.Article)
                .OrderBy(prod => prod.Name)
                .ThenBy(prod => prod.Price)
                .Take(4)
                .Select(prod => _mapper.Map<ProductCatalogDto>(prod))
                .ToListAsync(),
                Breadcrumbs = new List<BreadcrumbItem>()
                {
                    new BreadcrumbItem(product.Subcategory.Category.DisplayName, isActive:true, $"/catalog/{product.Subcategory.Category.Name}"),
                    new BreadcrumbItem(product.Subcategory.DisplayName, isActive:true, $"/catalog/{product.Subcategory.Category.Name}/{product.Subcategory.Name}"),
                    new BreadcrumbItem(product.Name, isActive:false),
                }
            };
            return View("Details", detailsCatalogViewModoel);
        }

        private IQueryable<ProductDto> GetFilteredProducts(int categoryId, int subcategoryId, int collectionId, string searchText, SortTypeEnum sortType = SortTypeEnum.popularity)
        {
            IQueryable<Product> query = _productsRepository.GetQuary();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.FullTextSearchQuery(searchText);
            }
            if (categoryId != 0)
            {
                query = query.Where(prod => prod.Subcategory.CategoryId == categoryId);
            }
            if (collectionId != 0)
            {
                query = query.Where(prod => prod.Collections.Any(col => col.CollectionId == collectionId));
            }
            if (subcategoryId != 0)
            {
                query = query.Where(prod => prod.SubcategoryId == subcategoryId);
            }
            if(sortType == SortTypeEnum.popularity)
            {
                query = query.OrderByDescending(prod => prod.CountOfTrasitions).ThenBy(prod=> prod.Name);
            }
            else if(sortType == SortTypeEnum.novelty)
            {
                query = query.OrderByDescending(prod => prod.CreationTime);
            }
            else if(sortType == SortTypeEnum.priceIncrease)
            {
                query = query.OrderBy(prod => prod.Price);
            }
            else if(sortType == SortTypeEnum.priceDecrease)
            {
                query = query.OrderByDescending(prod => prod.Price);
            }
            IQueryable<ProductDto> dtoQuery = query.Select(prod => _mapper.Map<ProductDto>(prod));
            return dtoQuery;
        }
    }
}
