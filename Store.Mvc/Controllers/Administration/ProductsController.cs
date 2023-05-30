using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.API.ViewModels.Administration;
using Store.API.ViewModels.Catalog;
using Store.Application.Dto.Administration;
using Store.Application.Dto.Product;
using Store.Application.Interfaces;
using Store.Domain.Interfaces;
using Store.Domain.Models.ManyToManyProductEntities;
using Store.Domain.Models.ProductEntities;
using System.Data;
using System.Runtime.CompilerServices;

namespace Store.API.Controllers.Administration
{
    [Authorize(Roles = "Moderator, Administrator")]
    public class ProductsController : Controller
    {
        private const int _pageSize = 10;

        private readonly IPicturesControl _picturesControl;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Size> _sizesRepository;
        private readonly IRepository<Color> _colorsRepository;
        private readonly IRepository<ClothingCollection> _collectionRepository;
        private readonly IArticleGenerator _articleGenerator;
        private readonly IMapper _mapper;

        public ProductsController(IPicturesControl picturesControl, IUnitOfWork unitOfWork, IRepository<Product> productRepository,
            IRepository<Size> sizesRepository, IRepository<Color> colorsRepository, IRepository<ClothingCollection> collectionRepository,
            IArticleGenerator articleGenerator, IMapper mapper)
        {
            _picturesControl = picturesControl;
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _sizesRepository = sizesRepository;
            _colorsRepository = colorsRepository;
            _collectionRepository = collectionRepository;
            _articleGenerator = articleGenerator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepository
                .GetQuary()
                .Select(prod => _mapper.Map<ProductDto>(prod))
                .ToListAsync();
            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> PaginationLine(int totalPage, int categoryId)
        {
            IQueryable<Product> products = _productRepository.GetQuary();
            if (categoryId != 0)
            {
                products = products.Where(prod => prod.Subcategory.CategoryId == categoryId);
            }
            var model = new PaginationLineViewModel()
            {
                CountOfPages = (int)Math.Ceiling(await products.CountAsync() / (double)_pageSize),
                TotalPage = totalPage
            };
            if (model.CountOfPages == 0)
            {
                return PartialView();
            }
            return PartialView(model);
        }

        [HttpGet]
        public async Task<IActionResult> ProductsTable(int totalPage, int categoryId)
        {
            IQueryable<Product> query = _productRepository.GetQuary();
            if (categoryId != 0)
            {
                query = query.Where(prod => prod.Subcategory.CategoryId == categoryId);
            }
            var products = await query
                .Skip((totalPage * _pageSize) - _pageSize)
                .Take(_pageSize)
                .OrderBy(prod => prod.Subcategory.CategoryId)
                .ThenBy(prod => prod.SubcategoryId)
                .Select(prod => _mapper.Map<ProductDto>(prod))
                .ToListAsync();
            return PartialView(products);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Add(ProductDto product = null)
        {
            ProductAddOrUpdateViewModel productViewModel = new ProductAddOrUpdateViewModel
            {
                Product = product,
                PageTitle = "Товары. Администрирование",
                PageHeader = "Добавить товар",
                ActionName = "add",
                ButtonName = "Добавить",
                Sizes = await _sizesRepository.GetQuary().OrderBy(size => size.Order).Select(size => _mapper.Map<SizeDto>(size)).ToListAsync(),
                Colors = await _colorsRepository.GetQuary().OrderBy(color => color.Order).Select(color => _mapper.Map<ColorDto>(color)).ToListAsync(),
                Collections = await _collectionRepository.GetQuary().OrderBy(collection => collection.Order).Select(col => _mapper.Map<CollectionDto>(col)).ToListAsync(),
            };
            return View("AddOrUpdateProduct", productViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductAddOrUpdateViewModel productViewModel)
        {
            if (productViewModel.MainImage == null && string.IsNullOrEmpty(productViewModel.Product.MainPicture))
            {
                ModelState.AddModelError("MainImage", "Необходимо добавить главное фото");
            }
            else if(productViewModel.MainImage != null)
            {
                try
                {
                    productViewModel.Product.MainPicture = await _picturesControl.UploadImage(productViewModel.MainImage, "images/main/");
                }
                catch
                {
                    ModelState.AddModelError("MainImage", "Ошибка при загрузке фото");
                    productViewModel.MainImage = null;
                }
            }
            if (productViewModel.AdditionalImages == null && productViewModel.Product.AdditionalPictures == null)
            {
                ModelState.AddModelError("AdditionalImages", "Необходимо добавить дополнительные фото");
            }
            else if (productViewModel.AdditionalImages != null)
            {
                try
                {
                    var imageLinks = await _picturesControl.UploadImage(productViewModel.AdditionalImages, "images/additional/");
                    productViewModel.Product.AdditionalPictures = imageLinks.ToArray();
                }
                catch
                {
                    ModelState.AddModelError("MainImage", "Ошибка при загрузке фото");
                    productViewModel.AdditionalImages = null;
                }
            }
            if (ProductValidation(productViewModel))
            {
                foreach (var collectionId in productViewModel.SelectedCollectionIds)
                {
                    productViewModel.Product.Collections.Add(new CollectionProduct(_mapper.Map<Product>(productViewModel.Product), collectionId));
                }
                foreach (var colorId in productViewModel.SelectedColorIds)
                {
                    productViewModel.Product.Colors.Add(new ColorProduct(_mapper.Map<Product>(productViewModel.Product), colorId));
                }
                foreach (var sizeId in productViewModel.SelectedSizeIds)
                {
                    productViewModel.Product.Sizes.Add(new ProductSize(_mapper.Map<Product>(productViewModel.Product), sizeId));
                }
                productViewModel.Product.Article = _articleGenerator.Generate();
                productViewModel.Product.CreationTime = DateTime.UtcNow;
                await _productRepository.AddAsync(_mapper.Map<Product>(productViewModel.Product));
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("index", "products");
            }
            productViewModel.Collections = await _collectionRepository.GetQuary().OrderBy(collection => collection.Order).Select(col => _mapper.Map<CollectionDto>(col)).ToListAsync();
            productViewModel.Sizes = await _sizesRepository.GetQuary().OrderBy(size => size.Order).Select(size => _mapper.Map<SizeDto>(size)).ToListAsync();
            productViewModel.Colors = await _colorsRepository.GetQuary().OrderBy(color => color.Order).Select(color => _mapper.Map<ColorDto>(color)).ToListAsync();
            return View("AddOrUpdateProduct", productViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Product product = await _productRepository.GetAsync(id);
            ProductAddOrUpdateViewModel productViewModel = new ProductAddOrUpdateViewModel
            {
                Product = _mapper.Map<ProductDto>(product),
                PageTitle = "Товары. Администрирование",
                PageHeader = "Изменить товар",
                ActionName = "update",
                ButtonName = "Изменить",
                Sizes = await _sizesRepository.GetQuary().OrderBy(size => size.Order).Select(size => _mapper.Map<SizeDto>(size)).ToListAsync(),
                Colors = await _colorsRepository.GetQuary().OrderBy(color => color.Order).Select(color => _mapper.Map<ColorDto>(color)).ToListAsync(),
                Collections = await _collectionRepository.GetQuary().OrderBy(collection => collection.Order).Select(col => _mapper.Map<CollectionDto>(col)).ToListAsync()
            };
            return View("AddOrUpdateProduct", productViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductAddOrUpdateViewModel productViewModel)
        {
            bool newMainImageUploaded = false;
            bool newAdditionalImageUploaded = false;

            if (productViewModel.MainImage == null && string.IsNullOrEmpty(productViewModel.Product.MainPicture))
            {
                ModelState.AddModelError("MainImage", "Необходимо добавить главное фото");
            }
            else if (productViewModel.MainImage != null)
            {

                try
                {
                    productViewModel.Product.MainPicture = await _picturesControl.UploadImage(productViewModel.MainImage, "images/main/");
                    newMainImageUploaded = true;
                }
                catch
                {
                    ModelState.AddModelError("MainImage", "Ошибка при загрузке фото");
                    productViewModel.MainImage = null;
                }
            }
            if (productViewModel.AdditionalImages == null && productViewModel.Product.AdditionalPictures == null)
            {
                ModelState.AddModelError("AdditionalImages", "Необходимо добавить дополнительные фото");
            }
            else if(productViewModel.AdditionalImages != null)
            {
                try
                {
                    var link = await _picturesControl.UploadImage(productViewModel.AdditionalImages, "images/additional/");
                    productViewModel.Product.AdditionalPictures = link.ToArray();
                    newAdditionalImageUploaded = true;
                }
                catch
                {
                    ModelState.AddModelError("MainImage", "Ошибка при загрузке фото");
                    productViewModel.AdditionalImages = null;
                }
            }
            if (ProductValidation(productViewModel))
            {
                try
                {
                    await _unitOfWork.BeginTransaction();
                    foreach (var collectionId in productViewModel.SelectedCollectionIds)
                    {
                        productViewModel.Product.Collections.Add(new CollectionProduct(_mapper.Map<Product>(productViewModel.Product), collectionId));
                    }
                    foreach (var colorId in productViewModel.SelectedColorIds)
                    {
                        productViewModel.Product.Colors.Add(new ColorProduct(_mapper.Map<Product>(productViewModel.Product), colorId));
                    }
                    foreach (var sizeId in productViewModel.SelectedSizeIds)
                    {
                        productViewModel.Product.Sizes.Add(new ProductSize(_mapper.Map<Product>(productViewModel.Product), sizeId));
                    }
                    if(productViewModel.Product.CreationTime == default(DateTime))
                    {
                        productViewModel.Product.CreationTime = DateTime.UtcNow;
                    }
                    var deleteItem = await _productRepository.GetAsync(productViewModel.Product.Id);
                    if (newMainImageUploaded)
                    {
                        _picturesControl.DeleteImages(deleteItem.MainPicture);
                    }
                    if (newAdditionalImageUploaded)
                    {
                        _picturesControl.DeleteImages(deleteItem.AdditionalPictures);
                    }
                    await _productRepository.DeleteAsync(productViewModel.Product.Id);
                    await _unitOfWork.SaveChangesAsync();
                    await _productRepository.AddAsync(_mapper.Map<Product>(productViewModel.Product));
                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitTransaction();
                }
                catch
                {
                    await _unitOfWork.RollbackTransaction();
                }

                return RedirectToAction("Index", "products");
            }
            productViewModel.Collections = await _collectionRepository.GetQuary().OrderBy(collection => collection.Order).Select(col => _mapper.Map<CollectionDto>(col)).ToListAsync();
            productViewModel.Sizes = await _sizesRepository.GetQuary().OrderBy(size => size.Order).Select(size => _mapper.Map<SizeDto>(size)).ToListAsync();
            productViewModel.Colors = await _colorsRepository.GetQuary().OrderBy(color => color.Order).Select(color => _mapper.Map<ColorDto>(color)).ToListAsync();
            return View("AddOrUpdateProduct", productViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _productRepository.GetAsync(id);
            _picturesControl.DeleteImages(product.MainPicture);
            _picturesControl.DeleteImages(product.AdditionalPictures);
            _productRepository.Delete(product);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("index", "products");
        }

        private bool ProductValidation(ProductAddOrUpdateViewModel productViewModel)
        {
            if (productViewModel.Product.SubcategoryId == 0)
            {
                ModelState.AddModelError("Product.SubcategoryId", "Необходимо выбрать категорию");
            }
            if (productViewModel.SelectedColorIds == null || productViewModel.SelectedColorIds.Length == 0)
            {
                ModelState.AddModelError("SelectedColorIds", "Необходимо выбрать хотя бы один цвет");
            }
            if (productViewModel.SelectedSizeIds == null || productViewModel.SelectedSizeIds.Length == 0)
            {
                ModelState.AddModelError("SelectedSizeIds", "Необходимо выбрать хотя бы один размер");
            }
            if (string.IsNullOrWhiteSpace(productViewModel.Product.Name))
            {
                ModelState.AddModelError("Product.Name", "Название товара не может быть пустым");
            }
            else if (productViewModel.Product.Name.Length > 50)
            {
                ModelState.AddModelError("Product.Name", "Длина названия не может превышать 50 символов");
            }
            if (productViewModel.Product.Price == 0)
            {
                ModelState.AddModelError("Product.Price", "Необходимо указать цену товара");
            }
            if (productViewModel.Product.Price < 199 || productViewModel.Product.Price > 99999)
            {
                ModelState.AddModelError("Product.Price", "Цена товара не может быть ниже 199 руб и выше 99999 руб");
            }
            if (ModelState.IsValid)
            {
                return true;
            }
            return false;
        }
    }
}
