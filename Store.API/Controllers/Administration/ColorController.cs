using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.API.ViewModels.Administration;
using Store.Application.Dto.Administration;
using Store.Domain.Interfaces;
using Store.Domain.Models.ProductEntities;
using System.Data;

namespace Store.API.Controllers.Administration
{
    [Authorize(Roles = "Moderator, Administrator")]
    public class ColorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Color> _colorRepository;
        private readonly IMapper _mapper;

        public ColorController(IUnitOfWork unitOfWork, IRepository<Color> colorRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _colorRepository = colorRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _colorRepository.GetQuary().ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _colorRepository.GetQuary()
                .OrderBy(color => color.Order)
                .Select(c => _mapper.Map<ColorDto>(c))
                .ToListAsync());
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult Add(ColorDto color = null)
        {
            ColorAddOrUpdateViewModel colorViewModel = new ColorAddOrUpdateViewModel
            {
                Color = color,
                PageTitle = "Цвета. Администрирование",
                PageHeader = "Добавить цвет",
                ActionName = "add",
                ButtonName = "Добавить",
            };
            return View("AddOrUpdateColor", colorViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Add(ColorAddOrUpdateViewModel colorViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_colorRepository.GetQuary().Any(col => col.Name.ToLower() == colorViewModel.Color.Name.ToLower()))
                {
                    ModelState.AddModelError("Color.Name", "Цвет с таким названием уже существует");
                }
                else
                {
                    await _colorRepository.AddAsync(_mapper.Map<Color>(colorViewModel.Color));
                    await _unitOfWork.SaveChangesAsync();
                    return RedirectToAction("Index", "color");
                }
            }
            return View("AddOrUpdateColor", colorViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Color color = await _colorRepository.GetAsync(id);
            ColorAddOrUpdateViewModel colorViewModel = new ColorAddOrUpdateViewModel
            {
                Color = _mapper.Map<ColorDto>(color),
                PageTitle = "Цвета. Администрирование",
                PageHeader = "Изменить цвет",
                ActionName = "update",
                ButtonName = "Изменить",
            };
            return View("AddOrUpdateColor", colorViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Update(ColorAddOrUpdateViewModel colorViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_colorRepository.GetQuary().Any(col => col.Name.ToLower() == colorViewModel.Color.Name.ToLower() && col.Id != colorViewModel.Color.Id))
                {
                    ModelState.AddModelError("Color.Name", "Цвет с таким названием уже существует");
                }
                else
                {
                    _colorRepository.Update(_mapper.Map<Color>(colorViewModel.Color));
                    await _unitOfWork.SaveChangesAsync();
                    return RedirectToAction("Index", "color");
                }
            }
            return View("AddOrUpdateColor", colorViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _colorRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index", "color");
        }
    }
}
