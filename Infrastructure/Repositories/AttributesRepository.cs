using Domain.Data.Entities;
using Infrastructure.Database;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AttributesRepository : IAttributesRepository
    {
        private readonly AppDbContext _context;

        public AttributesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Attributes>> GetAllAttributes(CancellationToken cancellationToken)
        {
            return await _context.Attributes.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Attributes> AddAttribute(Attributes attributes, CancellationToken cancellationToken)
        {
            _context.Attributes.Add(attributes);
            await _context.SaveChangesAsync(cancellationToken);
            return attributes;
        }

        public async Task<bool> AttributeExists(int attributeId)
        {
            return await _context.Attributes.AnyAsync(a => a.AttributeId == attributeId);
        }

        public IQueryable<Attributes> GetAttributesQuery()
        {
            return _context.Attributes.AsNoTracking();
        }
    }
}