using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.API.ViewModels.Administration;
using Store.Application.Dto;
using Store.Application.Dto.Administration;
using Store.Domain.DatabaseRepositories.Postgre;
using Store.Domain.Infrastructure;
using Store.Domain.Interfaces;
using Store.Domain.Models.ManyToManyProductEntities;
using Store.Domain.Models.ProductEntities;
using System.Data;

namespace Store.API.Controllers.Administration
{
    [Authorize(Roles = "Moderator, Administrator")]
    public class CollectionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ClothingCollection> _collectionRepository;
        private readonly IMapper _mapper;

        public CollectionController(IUnitOfWork unitOfWork, IRepository<ClothingCollection> collectionRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _collectionRepository = collectionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _collectionRepository.GetQuary().ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _collectionRepository.GetQuary()
                .OrderBy(col => col.Order)
                .Select(col => _mapper.Map<CollectionDto>(col))
                .ToListAsync());
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult Add(CollectionDto collection = null)
        {
            CollectionAddOrUpdateViewModel collectionViewModel = new CollectionAddOrUpdateViewModel
            {
                Collection = collection,
                PageTitle = "Коллекции. Администрирование",
                PageHeader = "Добавить коллекцию",
                ActionName = "add",
                ButtonName = "Добавить",
            };
            return View("AddOrUpdateCollection", collectionViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Add(CollectionAddOrUpdateViewModel collectionViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_collectionRepository.GetQuary().Any(col => col.Name.ToLower() == collectionViewModel.Collection.Name.ToLower()))
                {
                    ModelState.AddModelError("Collection.Name", "Коллекция с таким названием адресной строки уже существует");
                }
                if(_collectionRepository.GetQuary().Any(col => col.DisplayName.ToLower() == collectionViewModel.Collection.DisplayName.ToLower()))
                {
                    ModelState.AddModelError("Collection.DisplayName", "Коллекция с таким названием уже существует");
                }
                else
                {
                    await _collectionRepository.AddAsync(_mapper.Map<ClothingCollection>(collectionViewModel.Collection));
                    await _unitOfWork.SaveChangesAsync();
                    return RedirectToAction("Index", "collection");
                }
            }
            return View("AddOrUpdateCollection", collectionViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ClothingCollection collection = await _collectionRepository.GetAsync(id);
            CollectionAddOrUpdateViewModel collectionViewModel = new CollectionAddOrUpdateViewModel
            {
                Collection = _mapper.Map<CollectionDto>(collection),
                PageTitle = "Коллекции. Администрирование",
                PageHeader = "Изменить коллекцию",
                ActionName = "update",
                ButtonName = "Изменить",
            };
            return View("AddOrUpdateCollection", collectionViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Update(CollectionAddOrUpdateViewModel collectionViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_collectionRepository.GetQuary().Any(col => col.Name.ToLower() == collectionViewModel.Collection.Name.ToLower() && col.Id != collectionViewModel.Collection.Id))
                {
                    ModelState.AddModelError("Collection.Name", "Коллекция с таким названием адресной строки уже существует");
                }
                if (_collectionRepository.GetQuary().Any(col => col.DisplayName.ToLower() == collectionViewModel.Collection.DisplayName.ToLower() && col.Id != collectionViewModel.Collection.Id))
                {
                    ModelState.AddModelError("Collection.DisplayName", "Коллекция с таким названием уже существует");
                }
                else
                {
                    _collectionRepository.Update(_mapper.Map<ClothingCollection>(collectionViewModel.Collection));
                    await _unitOfWork.SaveChangesAsync();
                    return RedirectToAction("Index", "collection");
                }
            }
            return View("AddOrUpdateCollection", collectionViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _collectionRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index", "collection");
        }
    }
}
