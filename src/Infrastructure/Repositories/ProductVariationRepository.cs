using Domain.Data.Entities;
using Domain.Enum;
using Domain.Interface;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

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

        public async Task UpdateRangeAsync(IEnumerable<ProductVariations> variations, CancellationToken cancellationToken)
        {
            if (variations == null || !variations.Any())
                return;

            foreach (var variation in variations)
            {
                _context.ProductVariations.Update(variation);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteRangeAsync(List<ProductVariations> variations, CancellationToken cancellationToken)
        {
            _context.ProductVariations.RemoveRange(variations);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SoftDeleteByIdsAsync(IEnumerable<Guid> ids, Guid updatedBy, DateTime utcNow, CancellationToken ct)
        {
            if (ids == null) return 0;

            var affected = 0;

            foreach (var id in ids)
            {
                try
                {
                    var variation = await _context.ProductVariations
                        .FirstOrDefaultAsync(v => v.ProductVariationId == id, ct);

                    if (variation == null)
                        continue;
                    variation.Status = ProductVariationStatus.Deleted;
                    variation.UpdatedBy = updatedBy;
                    variation.UpdatedAt = utcNow;

                    await _context.SaveChangesAsync(ct);
                    affected++;
                }
                catch
                {
                }
            }

            return affected;
        }
    }
}