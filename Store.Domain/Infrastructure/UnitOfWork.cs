
using Store.Domain.Interfaces;

namespace Store.Domain.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private StoreContext dbContext;

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        private bool disposed = false;

        public StoreContext GetContext()
        {
            if (disposed)
            {
                dbContext = new StoreContext();
            }
            return dbContext;
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task BeginTransaction()
        {
            await dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransaction()
        {
            await dbContext.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransaction()
        {
            await dbContext.Database.RollbackTransactionAsync();
        }

        public UnitOfWork(StoreContext context)
        {
            dbContext = context;
        }

    }
}
