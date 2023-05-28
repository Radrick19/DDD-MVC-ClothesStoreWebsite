using Microsoft.EntityFrameworkCore;
using Store.Domain.Interfaces;
using Store.Domain.Models;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Store.Domain.DatabaseRepositories.Postgre
{
    public class BaseRepository<T> : IRepository<T> where T : Entity
    {
        protected StoreContext context => _unitOfWork.GetContext();
        protected readonly DbSet<T> dbSet;
        protected readonly IUnitOfWork _unitOfWork;

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            dbSet = context.Set<T>();
        }

        public virtual async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual async Task DeleteAsync(int entityId)
        {
            T entity = await dbSet.FindAsync(entityId);
            dbSet.Remove(entity);
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual IQueryable<T> GetQuary()
        {
            return dbSet;
        }

        public virtual void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await dbSet.FirstOrDefaultAsync(expression);
        }
    }
}
