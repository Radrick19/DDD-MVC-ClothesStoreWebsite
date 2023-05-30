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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PicturesControl(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadImage(IFormFile image, string picturesFolder)
        {
            string pictureUrl = picturesFolder + Guid.NewGuid() + Path.GetExtension(image.FileName);
            using (var fs = new FileStream(Path.Combine(_webHostEnvironment.WebRootPath, pictureUrl), FileMode.Create))
            {
                await image.CopyToAsync(fs);
            }
            return pictureUrl;
        }

        public async Task<List<string>> UploadImage(IEnumerable<IFormFile> images, string picturesFolder)
        {
            List<string> pictureUrls = new List<string>();
            foreach (var image in images)
            {
                string pictureUrl = picturesFolder + Guid.NewGuid() + Path.GetExtension(image.FileName);
                using(var fs = new FileStream(Path.Combine(_webHostEnvironment.WebRootPath, pictureUrl), FileMode.Create))
                {
                    await image.CopyToAsync(fs);
                }
                pictureUrls.Add(pictureUrl);
            }
            return pictureUrls;
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
