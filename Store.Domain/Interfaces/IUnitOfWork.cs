using Store.Domain.Models;

namespace Store.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        StoreContext GetContext();

        Task BeginTransaction();

        Task CommitTransaction();

        Task RollbackTransaction();

        void SaveChanges();

        Task SaveChangesAsync();
    }
}
