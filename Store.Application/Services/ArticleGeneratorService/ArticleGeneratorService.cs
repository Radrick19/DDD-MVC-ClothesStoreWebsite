using Store.Domain.Interfaces;
using Store.Domain.Models.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.ArticleGeneratorService
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
            StringBuilder stringBuilder = new StringBuilder();
            string articleResult = string.Empty;
            while (stringBuilder.Length < 6)
            {
                Random random = new Random();
                stringBuilder.Append(random.Next(0, 10));
            }
            articleResult = stringBuilder.ToString();
            if (_productRepository.GetQuary().Select(prod => prod.Article).Any(article => article == articleResult))
            {
                articleResult = Generate();
            }
            return articleResult;
        }
    }
}
