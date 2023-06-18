using Microsoft.EntityFrameworkCore;
using Store.Domain.Interfaces;
using Store.Domain.Models.ProductEntities;

namespace Store.Domain.DatabaseRepositories.Postgre
{
    public class SubcategoryRepository : BaseRepository<Subcategory>
    {
        public SubcategoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<Subcategory> GetAsync(int id)
        {
            return await context.Subcategories
                .Include(sc => sc.Category)
                .Include(sc=> sc.Products)
                .Where(sc=> sc.Id == id)
                .FirstAsync();
        }

        public override IQueryable<Subcategory> GetQuary()
        {
            return context.Subcategories
                .Include(sc => sc.Category)
                .Include(sc => sc.Products);
        }
    }
}
