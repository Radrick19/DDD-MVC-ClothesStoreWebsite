using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Store.Domain.Models;
using Store.Domain.Models.ProductEntities;
using Store.Mvc.Services.PicturesControlService.Enums;
using System.Collections.Generic;

namespace Store.Mvc.Services.PicturesControlService
{
    public class PicturesControlService : IPicturesControlService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Dictionary<PicturesType, string> pictureTypeLinks;

        public PicturesControlService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            pictureTypeLinks = new Dictionary<PicturesType, string>
            {
                { PicturesType.PromoPages, "images/promo/" },
                { PicturesType.MainProduct, "images/main/" },
                { PicturesType.AdditionalProduct, "images/additional/" }
            };
        }

        public async Task<string> UploadImage(IFormFile image, PicturesType pictureType)
        {
            string picturePath = pictureTypeLinks[pictureType] + Guid.NewGuid() + Path.GetExtension(image.FileName);
            string pathToSave = Path.Combine(_webHostEnvironment.WebRootPath, picturePath);
            using (var fs = new FileStream(pathToSave, FileMode.Create))
            {
                await image.CopyToAsync(fs);
            }
            return picturePath;
        }

        public async Task<List<string>> UploadImage(IEnumerable<IFormFile> images, PicturesType pictureType)
        {
            List<string> picturePathes = new List<string>();
            foreach (var image in images)
            {
                string picturePath = pictureTypeLinks[pictureType] + Guid.NewGuid() + Path.GetExtension(image.FileName);
                picturePathes.Add(picturePath);
                string pathToSave = Path.Combine(_webHostEnvironment.WebRootPath, picturePath);
                using (var fs = new FileStream(Path.Combine(_webHostEnvironment.WebRootPath, picturePath), FileMode.Create))
                {
                    await image.CopyToAsync(fs);
                }
            }
            return picturePathes;
        }

        public void DeleteImages(params string[] links)
        {
            foreach (var link in links)
            {
                string imageLink = Path.Combine(_webHostEnvironment.WebRootPath, link);
                if (File.Exists(imageLink))
                {
                    FileInfo image = new FileInfo(imageLink);
                    image.Delete();
                }
            }
        }
    }
}
