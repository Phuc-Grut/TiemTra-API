using Domain.Data.Entities;
using Domain.Interface;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryAttributesRepository : ICategoryAttributesRepository
    {
        private readonly AppDbContext _context;

        public CategoryAttributesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Attributes>> GetAttributesByCategory(int categoryId, CancellationToken cancellationToken)
        {
            return await _context.CategoryAttributes
                .Where(ca => ca.CategoryId == categoryId)
                .Include(ca => ca.Attribute)
                .Where(ca => ca.Attribute != null)
                .Select(ca => new Attributes
                {
                    AttributeId = ca.Attribute.AttributeId,
                    Name = ca.Attribute.Name,
                    Description = ca.Attribute.Description,
                    CreatedBy = ca.Attribute.CreatedBy,
                    UpdatedBy = ca.Attribute.UpdatedBy,
                    CreatedAt = ca.Attribute.CreatedAt,
                    UpdatedAt = ca.Attribute.UpdatedAt
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(int categoryId, int attributeId)
        {
            return await _context.CategoryAttributes
                .AnyAsync(ca => ca.CategoryId == categoryId && ca.AttributeId == attributeId);
        }

        public async Task RemoveAllCategoreyFromAttributes(int categoryId, CancellationToken cancellationToken)
        {
            var items = await _context.CategoryAttributes.Where(ca => ca.CategoryId == categoryId).ToListAsync(cancellationToken);

            _context.CategoryAttributes.RemoveRange(items);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public Task<int> CountAttributesByCategory(int categoryId, CancellationToken cancellationToken)
        {
            return _context.CategoryAttributes.Where(ca => ca.CategoryId == categoryId).CountAsync(cancellationToken);
        }

        public Task<List<int>> GetAttributeIdsByCategory(int categoryId, CancellationToken cancellationToken)
        {
            return _context.CategoryAttributes
                .Where(ca => ca.CategoryId == categoryId)
                .Select(ca => ca.AttributeId)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(CategoryAttributes categoryAttribute, CancellationToken cancellationToken)
        {
            await _context.CategoryAttributes.AddAsync(categoryAttribute, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveAsync(int categoryId, List<int> attributeIds, CancellationToken cancellationToken)
        {
            var toRemove = await _context.CategoryAttributes
                .Where(ca => ca.CategoryId == categoryId && attributeIds.Contains(ca.AttributeId))
                .ToListAsync(cancellationToken);

            if (toRemove.Any())
            {
                _context.CategoryAttributes.RemoveRange(toRemove);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task RemoveAttributeFromAllCategories(int attributeId, CancellationToken cancellationToken)
        {
            var toRemove = _context.CategoryAttributes
                            .Where(ca => ca.AttributeId == attributeId);

            _context.CategoryAttributes.RemoveRange(toRemove);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}