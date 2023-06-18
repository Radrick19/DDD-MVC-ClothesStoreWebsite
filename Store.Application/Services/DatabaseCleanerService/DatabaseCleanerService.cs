using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.Application.Dto.Account;
using Store.Domain.Interfaces;
using Store.Domain.Models;
using Store.Domain.Models.ProductEntities;

namespace Store.Application.Services.DatabaseCleanerService
{
    public class DatabaseCleanerService : IDatabaseCleanerService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<PromoPage> _promoPageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserEmailConfirmationHash> _userEmailConfrimHashRepositrory;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DatabaseCleanerService(IUnitOfWork unitOfWork, IRepository<User> userRepository, IRepository<UserEmailConfirmationHash> userEmailConfrimHashRepositrory, IWebHostEnvironment webHostEnvironment, IRepository<Product> productRepository, IRepository<PromoPage> promoPageRepository, IMapper mapper, ILogger<DatabaseCleanerService> logger)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _userEmailConfrimHashRepositrory = userEmailConfrimHashRepositrory;
            _webHostEnvironment = webHostEnvironment;
            _productRepository = productRepository;
            _promoPageRepository = promoPageRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task DeleteUnactivatedUsers()
        {
            var users = await _userRepository
                .GetQuary()
                .Where(user => DateTime.UtcNow - user.RegistrationDate >= TimeSpan.FromMinutes(10) && !user.IsEmailConfirmed)
                .ToListAsync();
            await _unitOfWork.BeginTransaction();
            foreach (var user in users)
            {
                _userRepository.Delete(user);
            }
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransaction();
        }

        public async Task DeleteUnactiveConfirmHashes()
        {
            var userHashes = await _userEmailConfrimHashRepositrory
                .GetQuary()
                .Where(ue => DateTime.UtcNow - ue.CreationDate >= TimeSpan.FromMinutes(10))
                .ToListAsync();
            await _unitOfWork.BeginTransaction();
            foreach (var userHash in userHashes)
            {
                _userEmailConfrimHashRepositrory.Delete(userHash);
            }
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransaction();
        }

        public async Task DeleteUnactivatedUser(int userId)
        {
            try
            {
                var userAndConfirmHash = _mapper.Map<UserDto>(await _userEmailConfrimHashRepositrory.FirstOrDefaultAsync(ue => ue.UserId == userId));
                var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == userId);
                await _userEmailConfrimHashRepositrory.DeleteAsync(userAndConfirmHash.Id);
                await _unitOfWork.SaveChangesAsync();
                if (!user.IsEmailConfirmed)
                {
                    _userRepository.Delete(user);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            catch(NullReferenceException ex) 
            {
                _logger.LogError("Can't execute (DeleteUnactivatedUserTask) :" + ex.Message);
            }
        }

        public async Task DeleteUnusedAdditionalProductPictures()
        {
            var pictures = new DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "images/additional")).GetFiles();
            var productsPictures = await _productRepository.GetQuary().Select(prod => prod.AdditionalPictures).ToListAsync();
            foreach (var picture in pictures)
            {
                var filePath = @"images/additional/" + picture.Name;
                bool isContainted = false;
                foreach (var product in productsPictures)
                {
                    if (product.Contains(filePath))
                    {
                        isContainted = true;
                        break;
                    }
                }
                if (!isContainted)
                {
                    picture.Delete();
                }
            }
        }

        public async Task DeleteUnusedMainProductPictures()
        {
            var pictures = new DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "images/main")).GetFiles();
            var pictureLinks = await _productRepository.GetQuary().Select(prod => prod.MainPicture).ToListAsync();
            foreach (var picture in pictures)
            {
                var filePath = @"images/main/" + picture.Name;
                if (!pictureLinks.Contains(filePath))
                {
                    picture.Delete();
                }
            }
        }

        public async Task DeleteUnusedPromoBgPictures()
        {
            var pictures = new DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "images/promo")).GetFiles();
            var pictureLinks = await _promoPageRepository.GetQuary().Select(promo => promo.PictureLink).ToListAsync();
            foreach (var picture in pictures)
            {
                var filePath = @"images/promo/" + picture.Name;
                if (!pictureLinks.Contains(filePath))
                {
                    picture.Delete();
                }
            }
        }
    }
}
