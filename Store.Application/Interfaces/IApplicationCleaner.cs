using Store.Domain.Interfaces;
using Store.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Interfaces
{
    public interface IApplicationCleaner
    {
        Task DeleteUnactivatedUser(int userId);
        Task DeleteUnactiveConfirmHashes();
        Task DeleteUnactivatedUsers();
        Task DeleteUnusedMainProductPictures();
        Task DeleteUnusedAdditionalProductPictures();
        Task DeleteUnusedPromoBgPictures();
    }
}
