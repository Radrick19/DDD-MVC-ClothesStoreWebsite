using Store.Domain.Models.ProductEntities;

namespace Store.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetByArticleAsync(string article);
    }
}
