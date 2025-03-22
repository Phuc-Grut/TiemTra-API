using Domain.Data.Entities;
using Infrastructure.Database;
using Infrastructure.Interface;
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

        public async Task AddAttributesToCategory(CategoryAttributes categoryAttribute, CancellationToken cancellationToken)
        {
            _context.CategoryAttributes.Add(categoryAttribute);
            await _context.SaveChangesAsync();
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

        public Task RemoveAttributesToCategory(CategoryAttributes categoryAttribute, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}