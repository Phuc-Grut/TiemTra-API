using Infrastructure.Database;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;
        public ProductRepository(AppDbContext dbContext)
        {
                _dbContext = dbContext;
        }

        public async Task<int> CountProductByCategory(int categoryId, CancellationToken cancellationToken)
        {
            return await _dbContext.Products.CountAsync(p => p.CategoryId == categoryId, cancellationToken);
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
