using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.ProductPopularityService
{
    public interface IProductPopularityService
    {
        Task IncreaseProductPopularityAsync(string article);
    }
}
