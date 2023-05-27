using Microsoft.EntityFrameworkCore;
using Store.Domain.Interfaces;
using Store.Domain.Models.ManyToManyProductEntities;
using Store.Domain.Models.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.DatabaseRepositories.Postgre
{
    public class CollectionProductRepository : BaseRepository<CollectionProduct>
    {
        public CollectionProductRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<CollectionProduct> GetAsync(int id)
        {
            return await context.CollectionsProducts
                .Include(cp => cp.Product)
                .ThenInclude(prod => prod.Subcategory)
                .Include(cp => cp.Collection)
                .Where(cp=> cp.ProductId == id)
                .FirstAsync();
        }

        public override IQueryable<CollectionProduct> GetQuary()
        {
            return context.CollectionsProducts
                .Include(cp => cp.Product)
                .ThenInclude(prod => prod.Subcategory)
                .Include(cp => cp.Collection);
        }
    }
}
