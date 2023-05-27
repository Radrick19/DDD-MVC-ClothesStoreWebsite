using Store.Domain.Models.ManyToManyProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Dto.Product
{
    public class ProductCatalogDto
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string MainPicture { get; set; }
        public string Article { get; set; }
        public ICollection<ColorProduct> Colors { get; set; }
    }
}
