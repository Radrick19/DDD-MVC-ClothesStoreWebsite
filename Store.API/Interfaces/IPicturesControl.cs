using Store.Domain.Models;
using Store.Domain.Models.ProductEntities;

namespace Store.Application.Interfaces
{
    public interface IPicturesControl
    {
        Task<string> UploadImage(IFormFile image, string picturesFolder);
        Task<List<string>> UploadImage(IEnumerable<IFormFile> images, string picturesFolder);
        Task DeleteImages(params string[] links);
    }
}
