using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.API.ViewModels.Administration;
using Store.API.ViewModels.Catalog;
using Store.Application.Dto.Administration;
using Store.Domain.DatabaseRepositories.Postgre;
using Store.Domain.Interfaces;
using Store.Domain.Models.ProductEntities;
using System.Data;

namespace Store.API.Controllers.Administration
{
    [Authorize(Roles = "Moderator, Administrator")]
    public class CategoryController : Controller
    {
        private readonly int _pageSize = 10;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Subcategory> _subcategryRepository;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork unitOfWork, IRepository<Category> categoryRepository,
            IRepository<Subcategory> subcategryRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _subcategryRepository = subcategryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> PaginationLine(int totalPage, int categoryId)
        {
            IQueryable<Subcategory> quary = _subcategryRepository.GetQuary();
            if (categoryId != 0)
            {
                quary = quary.Where(prod => prod.CategoryId == categoryId);
            }
            var model = new PaginationLineViewModel()
            {
                CountOfPages = (int)Math.Ceiling(await quary.CountAsync() / (double)_pageSize),
                TotalPage = totalPage
            };
            if (model.CountOfPages == 0)
            {
                return PartialView();
            }
            return PartialView(model);
        }

        [HttpGet]
        public async Task<IActionResult> SubcategoriesTable(int totalPage, int categoryId)
        {
            IQueryable<Subcategory> query = _subcategryRepository.GetQuary();
            if (categoryId != 0)
            {
                query = query.Where(prod => prod.CategoryId == categoryId);
            }

            var subcategories = await query
                .Skip((totalPage * _pageSize) - _pageSize)
                .Take(_pageSize)
                .OrderBy(prod => prod.CategoryId)
                .ThenBy(prod => prod.Name)
                .Select(sc => _mapper.Map<SubcategoryDto>(sc))
                .ToListAsync();

            return PartialView(subcategories);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await _categoryRepository.GetQuary().Select(c => _mapper.Map<CategoryDto>(c)).ToListAsync());

        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubcategories(int categoryId)
        {
            return Ok(await _subcategryRepository
                .GetQuary()
                .Where(sc => sc.CategoryId == categoryId)
                .Select(sc => _mapper.Map<SubcategoryDto>(sc))
                .ToListAsync());
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult Add(SubcategoryDto subcategory = null)
        {
            SubcategoryAddOrUpdateViewModel subcategoryViewModel = new SubcategoryAddOrUpdateViewModel
            {
                Subcategory = subcategory,
                PageTitle = "Подкатегории. Администрирование",
                PageHeader = "Добавить подкатегорию",
                ActionName = "add",
                ButtonName = "Добавить",
                Categories = _categoryRepository.GetQuary().Select(c => _mapper.Map<CategoryDto>(c))
            };
            return View("AddOrUpdateSubcategory", subcategoryViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Add(SubcategoryAddOrUpdateViewModel subcategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_subcategryRepository.GetQuary()
                    .Where(sc => sc.CategoryId == subcategoryViewModel.Subcategory.CategoryId)
                    .Any(sc => sc.DisplayName.ToLower() == subcategoryViewModel.Subcategory.DisplayName.ToLower()))
                {
                    ModelState.AddModelError("Subcategory.DisplayName", "Данное название подкатегории уже существует");
                }
                if (_subcategryRepository.GetQuary()
                    .Where(sc => sc.CategoryId == subcategoryViewModel.Subcategory.CategoryId)
                    .Any(sc => sc.Name.ToLower() == subcategoryViewModel.Subcategory.Name.ToLower()))
                {
                    ModelState.AddModelError("Subcategory.Name", "Данное название подкатегори в адресной строке уже существует");
                }
                else
                {
                    await _subcategryRepository.AddAsync(_mapper.Map<Subcategory>(subcategoryViewModel.Subcategory));
                    await _unitOfWork.SaveChangesAsync();
                    return RedirectToAction("Index", "category");
                }
            }
            subcategoryViewModel.Categories = _categoryRepository.GetQuary().Select(c => _mapper.Map<CategoryDto>(c));
            return View("AddOrUpdateSubcategory", subcategoryViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var subcategory = await _subcategryRepository.GetAsync(id);
            SubcategoryAddOrUpdateViewModel subcategoryViewModel = new SubcategoryAddOrUpdateViewModel
            {
                Subcategory = _mapper.Map<SubcategoryDto>(subcategory),
                PageTitle = "Подкатегории. Администрирование",
                PageHeader = "Изменить подкатегорию",
                ActionName = "update",
                ButtonName = "Изменить",
                Categories = _categoryRepository.GetQuary().Select(c => _mapper.Map<CategoryDto>(c))
            };
            return View("AddOrUpdateSubcategory", subcategoryViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Update(SubcategoryAddOrUpdateViewModel subcategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_subcategryRepository.GetQuary()
                    .Where(sc => sc.CategoryId == subcategoryViewModel.Subcategory.CategoryId)
                    .Any(sc => sc.Name.ToLower() == subcategoryViewModel.Subcategory.Name.ToLower() && sc.Id != subcategoryViewModel.Subcategory.Id))
                {
                    ModelState.AddModelError("Subcategory.Name", "Данное название подкатегории уже существует");
                }
                else
                {
                    _subcategryRepository.Update(_mapper.Map<Subcategory>(subcategoryViewModel.Subcategory));
                    await _unitOfWork.SaveChangesAsync();
                    return RedirectToAction("Index", "category");
                }
            }
            subcategoryViewModel.Categories = _categoryRepository.GetQuary().Select(c => _mapper.Map<CategoryDto>(c));
            return View("AddOrUpdateSubcategory", subcategoryViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _subcategryRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index", "category");
        }
    }
}