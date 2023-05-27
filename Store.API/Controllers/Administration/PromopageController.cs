using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.API.ViewModels.Administration;
using Store.Application.Dto.Administration;
using Store.Application.Interfaces;
using Store.Domain.Interfaces;
using Store.Domain.Models;
using System.Data;

namespace Store.API.Controllers.Administration
{
    [Authorize(Roles = "Administrator")]
    public class PromopageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<PromoPage> _promoPageRepository;
        private readonly IPicturesControl _picturesControl;
        private readonly IMapper _mapper;

        public PromopageController(IUnitOfWork unitOfWork, IRepository<PromoPage> promoPageRepository, IPicturesControl picturesControl, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _promoPageRepository = promoPageRepository;
            _picturesControl = picturesControl;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _promoPageRepository.GetQuary().ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _promoPageRepository
                .GetQuary()
                .OrderBy(pp => pp.Order)
                .Select(pp=> _mapper.Map<PromoPageDto>(pp))
                .ToListAsync());
        }

        [HttpGet]
        public IActionResult Add(PromoPageDto promoPage = null)
        {
            PromoPageAddOrUpdateViewModel promoPageViewModel = new PromoPageAddOrUpdateViewModel
            {
                PromoPage = promoPage,
                PageTitle = "Промо страницы. Администрирование",
                PageHeader = "Добавить промо страницу",
                ActionName = "add",
                ButtonName = "Добавить",
            };
            return View("AddOrUpdatePromopage", promoPageViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(PromoPageAddOrUpdateViewModel promoPageViewModel)
        {
            if (_promoPageRepository.GetQuary().Any(pp => pp.Title.ToLower() == promoPageViewModel.PromoPage.Title.ToLower()))
            {
                ModelState.AddModelError("PromoPage.Title", "Промостраница с таким заголовком уже существует");
            }
            if (promoPageViewModel.Image == null && string.IsNullOrEmpty(promoPageViewModel.PromoPage.PictureLink))
            {
                ModelState.AddModelError("MainImage", "Необходимо добавить фото");
            }
            else if (promoPageViewModel.Image != null)
            {
                try
                {
                    promoPageViewModel.PromoPage.PictureLink = await _picturesControl.UploadImage(promoPageViewModel.Image, "images/promo/");
                }
                catch
                {
                    ModelState.AddModelError("MainImage", "Ошибка при загрузке фото");
                    promoPageViewModel.Image = null;
                }
            }
            if (ModelState.IsValid)
            {
                await _promoPageRepository.AddAsync(_mapper.Map<PromoPage>(promoPageViewModel.PromoPage));
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index", "promopage");
            }
            return View("AddOrUpdatePromopage", promoPageViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            PromoPage promoPage = await _promoPageRepository.GetAsync(id);
            PromoPageAddOrUpdateViewModel promoPageViewModel = new PromoPageAddOrUpdateViewModel
            {
                PromoPage = _mapper.Map<PromoPageDto>(promoPage),
                PageTitle = "Промо страницы. Администрирование",
                PageHeader = "Изменить промостраницу",
                ActionName = "update",
                ButtonName = "Изменить",
            };
            return View("AddOrUpdatePromopage", promoPageViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PromoPageAddOrUpdateViewModel promoPageViewModel)
        {
            if (promoPageViewModel.Image == null && string.IsNullOrEmpty(promoPageViewModel.PromoPage.PictureLink))
            {
                ModelState.AddModelError("PictureLink", "Необходимо добавить фото");
            }
            else if (promoPageViewModel.Image != null)
            {
                try
                {
                    promoPageViewModel.PromoPage.PictureLink = await _picturesControl.UploadImage(promoPageViewModel.Image, "images/promo/");
                }
                catch
                {
                    ModelState.AddModelError("MainImage", "Ошибка при загрузке фото");
                    promoPageViewModel.Image = null;
                }
            }
            if (_promoPageRepository.GetQuary().Any(pp => pp.Title.ToLower() == promoPageViewModel.PromoPage.Title.ToLower() && pp.Id != promoPageViewModel.PromoPage.Id))
            {
                ModelState.AddModelError("PromoPage.Title", "Промостраница с таким заголовком уже существует");
            }
            if (ModelState.IsValid)
            {
                _promoPageRepository.Update(_mapper.Map<PromoPage>(promoPageViewModel.PromoPage));
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index", "promopage");
            }
            return View("AddOrUpdatePromopage", promoPageViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            PromoPage promoPage = await _promoPageRepository.GetAsync(id);
            await _picturesControl.DeleteImages(promoPage.PictureLink);
            await _promoPageRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index", "promopage");
        }
    }
}
