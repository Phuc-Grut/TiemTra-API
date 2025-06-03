using Domain.Data.Entities;
using Domain.Interface;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductAttributeRepository : IProductAttributeRepository
    {
        private readonly AppDbContext _context;
        public ProductAttributeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddRangeAsync(List<ProductAttribute> productAttributes, CancellationToken cancellationToken)
        {
            if (productAttributes == null || !productAttributes.Any())
            {
                return;
            }

            await _context.ProductAttributes.AddRangeAsync(productAttributes, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteByProductIdAsync(Guid productId, CancellationToken cancellationToken)
        {
            var attributes = await _context.ProductAttributes
                .Where(attr => attr.ProductId == productId).ToListAsync();

            if (attributes.Any())
            {
                _context.ProductAttributes.RemoveRange(attributes);
                await _context.SaveChangesAsync(cancellationToken); 
            }
        }
    }
}
