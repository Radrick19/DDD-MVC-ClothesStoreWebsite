using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Store.Application.Interfaces;
using Store.Domain.Models;
using Store.Domain.Models.ProductEntities;
using System.Collections.Generic;

namespace Store.API.Infrastructure
{
    public class PicturesControl : IPicturesControl
    { 
        IWebHostEnvironment _webHostEnvironment;

        public PicturesControl(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadImage(IFormFile image, string picturesFolder)
        {
            string picturePath = picturesFolder + Guid.NewGuid() + Path.GetExtension(image.FileName);
            string pathToSave = Path.Combine(_webHostEnvironment.WebRootPath, picturePath);
            using (var fs = new FileStream(pathToSave, FileMode.Create))
            {
                await image.CopyToAsync(fs);
            }
            return picturePath;
        }

        public async Task<List<string>> UploadImage(IEnumerable<IFormFile> images, string picturesFolder)
        {
            List<string> picturePathes = new List<string>();
            foreach (var image in images)
            {
                string picturePath = picturesFolder + Guid.NewGuid() + Path.GetExtension(image.FileName);
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
