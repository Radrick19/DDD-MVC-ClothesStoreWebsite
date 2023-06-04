using Store.Domain.Models;
using Store.Domain.Models.ProductEntities;
using Store.Mvc.Services.PicturesControlService.Enums;

namespace Store.Mvc.Services.PicturesControlService
{
    public interface IPicturesControlService
    {
        Task<string> UploadImage(IFormFile image, PicturesType pictureType);
        Task<List<string>> UploadImage(IEnumerable<IFormFile> images, PicturesType pictureType);
        void DeleteImages(params string[] links);
    }
}
