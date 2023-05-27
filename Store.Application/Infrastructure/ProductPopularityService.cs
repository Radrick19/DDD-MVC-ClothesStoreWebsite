using Microsoft.AspNetCore.Authentication;
using Store.Application.Interfaces;
using Store.Domain.Interfaces;
using Store.Domain.Models.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Infrastructure
{
    public class ProductPopularityService : IProductPopularityService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductPopularityService(IRepository<Product> productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task IncreaseProductPopularityAsync(string article)
        {
            var product = await (_productRepository as IProductRepository).GetByArticleAsync(article);
            product.CountOfTrasitions++;
            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
