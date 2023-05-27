using Store.Application.Dto.Administration;
using Store.Domain.Models.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Models.Personal
{
    public class CookieCartItem
    {
        public string Article { get; set; }
        public ColorDto Color { get; set; }
        public SizeDto Size { get; set; }
        public int Quantity { get; set; }
    }
}
