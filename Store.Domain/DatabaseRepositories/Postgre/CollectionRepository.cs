using Microsoft.EntityFrameworkCore;
using Store.Domain.Interfaces;
using Store.Domain.Models.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.DatabaseRepositories.Postgre
{
    public class CollectionRepository : BaseRepository<ClothingCollection>
    {
        public CollectionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async override Task<ClothingCollection> GetAsync(int id)
        {
            return await context.ClothingCollections
                .Include(col => col.Products)
                .Where(col => col.Id == id)
                .FirstAsync();
        }

        public override IQueryable<ClothingCollection> GetQuary()
        {
            return context.ClothingCollections
                .Include(col => col.Products);
        }
    }
}
