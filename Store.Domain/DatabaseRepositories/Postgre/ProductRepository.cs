using Microsoft.EntityFrameworkCore;
using Store.Domain.Interfaces;
using Store.Domain.Models.ManyToManyProductEntities;
using Store.Domain.Models.ProductEntities;

namespace Store.Domain.DatabaseRepositories.Postgre
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<Product> GetAsync(int id)
        {
            return await context.Products
                .Include(prod => prod.Collections)
                .ThenInclude(col => col.Collection)
                .Include(prod => prod.Subcategory)
                .ThenInclude(sc => sc.Category)
                .Include(prod => prod.Colors)
                .ThenInclude(color => color.Color)
                .Include(prod => prod.Sizes)
                .ThenInclude(size => size.Size)
                .Where(prod => prod.Id == id)
                .FirstAsync();
        }

        public override IQueryable<Product> GetQuary()
        {
            return context.Products
                .Include(prod => prod.Collections)
                .ThenInclude(col => col.Collection)
                .Include(prod => prod.Subcategory)
                .ThenInclude(sc => sc.Category)
                .Include(prod => prod.Colors)
                .ThenInclude(color => color.Color)
                .Include(prod => prod.Sizes)
                .ThenInclude(size => size.Size);
        }

        public async Task<Product> GetByArticleAsync(string article)
        {
            return await context.Products
                .Include(prod => prod.Collections)
                .ThenInclude(col => col.Collection)
                .Include(prod => prod.Subcategory)
                .ThenInclude(sc => sc.Category)
                .Include(prod => prod.Colors)
                .ThenInclude(color => color.Color)
                .Include(prod => prod.Sizes)
                .ThenInclude(size => size.Size)
                .Where(prod => prod.Article == article)
                .FirstAsync();
        }
    }
}
