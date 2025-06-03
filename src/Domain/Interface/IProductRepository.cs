using Domain.Data.Entities;
using Domain.DTOs.Product;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IProductRepository
    {
        Task<int> CountProductByCategory(int categoryId, CancellationToken cancellationToken);
        Task RemoveCategoryFromProducts(int categoryId, CancellationToken cancellationToken);
        Task AddAsync(Product product, CancellationToken cancellationToken);
        Task<bool> ProductCodeExistsAsync(string productCode);
        IQueryable<Product> GetFilteredProducts(ProductFilterDto filters, CancellationToken cancellationToken);
        Task<Product> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken);
        Task<bool> UpdateProduct(Product product, CancellationToken cancellationToken);
    }
}
