using Microsoft.EntityFrameworkCore;
using Store.Domain.Interfaces;
using Store.Domain.Migrations;
using Store.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.DatabaseRepositories.Postgre
{
    public class UserEmailConfirmationHashRepository : BaseRepository<UserEmailConfirmationHash>
    {
        public UserEmailConfirmationHashRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override IQueryable<UserEmailConfirmationHash> GetQuary()
        {
            return dbSet.Include(ue=> ue.User);
        }

        public async override Task<UserEmailConfirmationHash> GetAsync(int id)
        {
            return await dbSet.Include(ue => ue.User).FirstOrDefaultAsync(ue=> ue.Id == id);
        }

        public async override Task<UserEmailConfirmationHash> FirstOrDefaultAsync(Expression<Func<UserEmailConfirmationHash, bool>> expression)
        {
            return await dbSet.Include(ue => ue.User).FirstOrDefaultAsync(expression);
        }
    }
}
