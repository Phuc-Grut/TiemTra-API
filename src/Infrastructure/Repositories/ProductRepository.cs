using Domain.Data.Entities;
using Infrastructure.Database;
using Domain.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTOs.Product;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;
        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken)
        {
            await _dbContext.Products.AddAsync(product, cancellationToken);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CountProductByCategory(int categoryId, CancellationToken cancellationToken)
        {
            return await _dbContext.Products.CountAsync(p => p.CategoryId == categoryId, cancellationToken);
        }

        public  IQueryable<Product> GetFilteredProducts(ProductFilterDto filters, CancellationToken cancellationToken)
        {
            var query = _dbContext.Products.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filters.ProductCode))
            {
                var code = filters.ProductCode.Trim();
                query = query.Where(p => p.ProductCode.Contains(code));
            }

            if (!string.IsNullOrWhiteSpace(filters.Keyword))
            {
                var keyword = filters.Keyword.Trim();
                query = query.Where(p => p.ProductName.Contains(keyword) || p.ProductCode.Contains(keyword));
            }

            if (filters.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == filters.CategoryId.Value);
            }

            if (filters.BrandId.HasValue)
            {
                query = query.Where(p => p.BrandId == filters.BrandId.Value);
            }

            if (filters.Status.HasValue)
            {
                query = query.Where(p => p.ProductStatus == filters.Status.Value);
            }

            var sortFields = (filters.SortBy ?? "")
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim().ToLower())
                .ToList();

            bool isFirstSort = true;

            foreach (var sort in sortFields)
            {
                if (sort == "price-asc")
                    query = isFirstSort ? query.OrderBy(p => p.Price) : ((IOrderedQueryable<Product>)query).ThenBy(p => p.Price);
                else if (sort == "price-desc")
                    query = isFirstSort ? query.OrderByDescending(p => p.Price) : ((IOrderedQueryable<Product>)query).ThenByDescending(p => p.Price);
                else if (sort == "sold-asc")
                    query = isFirstSort ? query.OrderBy(p => p.TotalSold) : ((IOrderedQueryable<Product>)query).ThenBy(p => p.TotalSold);
                else if (sort == "sold-desc")
                    query = isFirstSort ? query.OrderByDescending(p => p.TotalSold) : ((IOrderedQueryable<Product>)query).ThenByDescending(p => p.TotalSold);

                else if (sort == "createAt-desc")
                    query = isFirstSort ? query.OrderByDescending(p => p.CreatedAt) : ((IOrderedQueryable<Product>)query).ThenByDescending(p => p.CreatedAt);

                else if (sort == "createAt-asc")
                    query = isFirstSort ? query.OrderBy(p => p.CreatedAt) : ((IOrderedQueryable<Product>)query).ThenBy(p => p.CreatedAt);
                isFirstSort = false;
            }

            // Nếu không có sắp xếp nào hợp lệ → mặc định
            if (isFirstSort)
            {
                query = query.OrderByDescending(p => p.CreatedAt);
            }


            return query;

        }


        public Task<bool> ProductCodeExistsAsync(string productCode)
        {
            return _dbContext.Products.AsNoTracking().AnyAsync(p => p.ProductCode == productCode);
        }

        public async Task RemoveCategoryFromProducts(int categoryId, CancellationToken cancellationToken)
        {
            var products = await _dbContext.Products.Where(p => p.CategoryId == categoryId).ToListAsync(cancellationToken);

            foreach (var product in products)
            {
                product.CategoryId = null;
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
