using Domain.Data.Entities;
using Domain.Interface;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductVariationRepository : IProductVariationRepository
    {
        private readonly AppDbContext _context;
        public ProductVariationRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddRangeAsync(List<ProductVariations> productVariations, CancellationToken cancellationToken)
        {
            if (productVariations == null || !productVariations.Any())
            {
                return;
            }

            await _context.ProductVariations.AddRangeAsync(productVariations, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteByProductIdAsync(Guid productId, CancellationToken cancellationToken)
        {
            var variations = await _context.ProductVariations
                .Where(v => v.ProductId == productId).ToListAsync(cancellationToken);

            if (variations.Any())
            {
                _context.ProductVariations.RemoveRange(variations);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<ProductVariations?> GetByIdAsync(Guid? productVariationId, CancellationToken cancellationToken)
        {
            return await _context.ProductVariations
                .Include(v => v.Product)
                .FirstOrDefaultAsync(v => v.ProductVariationId == productVariationId, cancellationToken);
        }

        public async Task UpdateQuantityAsync(Guid productVariationId, int newStock, CancellationToken cancellationToken)
        {
            var variation = new ProductVariations
            {
                ProductVariationId = productVariationId,
                Stock = newStock
            };

            _context.ProductVariations.Attach(variation);
            _context.Entry(variation).Property(v => v.Stock).IsModified = true;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<ProductVariations>> GetAllByProductIdAsync(Guid productId, CancellationToken cancellationToken)
        {
            return await _context.ProductVariations
                .Where(v => v.ProductId == productId)
                .ToListAsync(cancellationToken);
        }

    }
}
