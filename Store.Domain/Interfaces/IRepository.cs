using Store.Domain.Models;
using System.Linq.Expressions;

namespace Store.Domain.Interfaces
{
    public interface IRepository<TEntity, TPrimaryKey> where TEntity : Entity<TPrimaryKey>
    {
        public IQueryable<TEntity> GetQuary();
        public Task<TEntity> GetAsync(int id);
        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> func);
        public Task AddAsync(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(TEntity entity);
        public Task DeleteAsync(int entityId);
    }

    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : Entity
    {

    }
}
