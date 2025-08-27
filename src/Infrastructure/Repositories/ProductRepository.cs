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
using Domain.Enum;

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
            var query = _dbContext.Products
                .AsNoTracking()
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .Include(p => p.ProductVariations.Where(v => v.Status != ProductVariationStatus.Deleted))
                .Where(p => p.ProductStatus != ProductStatus.Deleted && p.ProductStatus != ProductStatus.Draft && p.ProductStatus != ProductStatus.Inactive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filters.ProductCode))
            {
                var code = filters?.ProductCode.Trim();
                query = query.Where(p => p.ProductCode.Contains(code));
            }

            if (!string.IsNullOrWhiteSpace(filters.Keyword))
            {
                var keyword = filters?.Keyword.Trim();
                query = query.Where(p => p.ProductName.Contains(keyword) || p.ProductCode.Contains(keyword));
            }

            if (filters.CategoryIds != null && filters.CategoryIds.Any())
            {
                query = query.Where(p => filters.CategoryIds.Contains(p.CategoryId.Value));
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

                else if (sort == "totalsold-asc")
                    query = isFirstSort ? query.OrderBy(p => p.TotalSold ?? 0) : ((IOrderedQueryable<Product>)query).ThenBy(p => p.TotalSold ?? 0);
                else if (sort == "totalsold-desc")
                    query = isFirstSort ? query.OrderByDescending(p => p.TotalSold ?? 0) : ((IOrderedQueryable<Product>)query).ThenByDescending(p => p.TotalSold ?? 0);

                else if (sort == "stock-asc")
                    query = isFirstSort ? query.OrderBy(p => p.Stock) : ((IOrderedQueryable<Product>)query).ThenBy(p => p.Stock);
                else if (sort == "stock-desc")
                    query = isFirstSort ? query.OrderByDescending(p => p.Stock) : ((IOrderedQueryable<Product>)query).ThenByDescending(p => p.Stock);

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

        public async Task<Product> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                .AsSplitQuery()
                .Include(p => p.ProductImages)
                .Include(p => p.ProductVariations.Where(v => v.Status != ProductVariationStatus.Deleted))
                .Include(p => p.ProductAttributes)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == productId, cancellationToken);

            return product;
        }

        public async Task<bool> ProductCodeExistsAsync(string productCode)
        {
            return await _dbContext.Products.AsNoTracking().AnyAsync(p => p.ProductCode == productCode);
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
        public async Task<bool> UpdateProduct(Product product, CancellationToken cancellationToken)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Store Product
        /// </summary>

        public async Task<Product> GetProductByCodeAsync(string productCode, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                .AsNoTracking()
                .AsSplitQuery()
                .Include(p => p.ProductImages)
                .Include(p => p.ProductVariations.Where(v => v.Status != ProductVariationStatus.Deleted))
                .Include(p => p.ProductAttributes!)
                    .ThenInclude(pa => pa.Attribute)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductCode == productCode, cancellationToken);

            return product;
        }

        public async Task UpdateQuantityAsync(Guid productId, CancellationToken cancellationToken)
        {
            var totalStock = await _dbContext.ProductVariations
                .Where(v => v.ProductId == productId)
                .SumAsync(v => v.Stock ?? 0, cancellationToken);

            var product = new Product
            {
                ProductId = productId,
                Stock = totalStock
            };
            _dbContext.Products.Attach(product);
            _dbContext.Entry(product).Property(p => p.Stock).IsModified = true;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateSoldQuantityAsync(Guid productId, int soldQuantity, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                ProductId = productId,
                TotalSold = soldQuantity
            };

            _dbContext.Products.Attach(product);
            _dbContext.Entry(product).Property(p => p.TotalSold).IsModified = true;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Product>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            var idList = ids.Distinct().ToList();
            if (idList.Count == 0) return new List<Product>();

            return await _dbContext.Products
                .Where(p => idList.Contains(p.ProductId))
                .ToListAsync(ct);
        }

        public async Task<int> SoftDeleteByIdsAsync( IEnumerable<Guid> ids, Guid updatedBy, DateTime utcNow, CancellationToken ct)
        {
            var idList = ids?.Where(x => x != Guid.Empty).Distinct().ToList() ?? new();
            if (idList.Count == 0) return 0;

            // Tải những sản phẩm chưa bị xóa
            var products = await _dbContext.Products
                .Where(p => idList.Contains(p.ProductId) && p.ProductStatus != ProductStatus.Deleted)
                .ToListAsync(ct);

            if (products.Count == 0) return 0;

            foreach (var p in products)
            {
                p.ProductStatus = ProductStatus.Deleted;

                p.UpdatedAt = utcNow;
                p.UpdatedBy = updatedBy;
            }

            await _dbContext.SaveChangesAsync(ct);
            return products.Count;
        }

    }
}
