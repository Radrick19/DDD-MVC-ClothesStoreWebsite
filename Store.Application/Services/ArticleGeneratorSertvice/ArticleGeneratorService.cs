using Store.Domain.Interfaces;
using Store.Domain.Models.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.ArticleGeneratorSertvice
{
    public class ArticleGeneratorService : IArticleGeneratorService
    {
        private readonly IRepository<Product> _productRepository;

        public ArticleGeneratorService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public string Generate()
        {
            StringBuilder articleBuilder = new StringBuilder();
            string returnResult = string.Empty;
            while (articleBuilder.Length < 6)
            {
                Random random = new Random();
                articleBuilder.Append(random.Next(0, 10));
            }
            returnResult = articleBuilder.ToString();
            if (_productRepository.GetQuary().Select(prod => prod.Article).Any(article => article == returnResult))
            {
                returnResult = Generate();
            }
            return returnResult;
        }
    }
}
