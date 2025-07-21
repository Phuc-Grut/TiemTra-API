using Domain.Data.Entities;
using Infrastructure.Database;
using Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategories(CancellationToken cancellationToken)
        {
            return await _context.Categories
                .Include(c => c.ChildCategories)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Category> AddCategory(Category category, CancellationToken cancellationToken)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync(cancellationToken);
            return category;
        }

        public async Task<bool> DeleteCategory(int categoryId, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId, cancellationToken);

            if (category == null)
            {
                return false;
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public IQueryable<Category> GetCategoriesQuery()
        {
            return _context.Categories.AsQueryable();
        }

        public async Task<Category> GetCategoryById(int categoryId, CancellationToken cancellationToken)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId, cancellationToken);
        }

        public async Task<bool> UpdateCategory(Category category, CancellationToken cancellationToken)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> HasChildCategories(int categoryId, CancellationToken cancellationToken)
        {
            return await _context.Categories.AnyAsync(c => c.ParentId == categoryId);
        }

        public async Task<bool> CategoryExists(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.CategoryId == categoryId);
        }

        public async Task<List<Category>> GetSubCategories(int parentId, CancellationToken cancellationToken)
        {
            return await _context.Categories
                .Where(c => c.ParentId == parentId)
                .ToListAsync();
        }

        public Task<List<Category>> GetLeafCategoriesAsync(CancellationToken cancellationToken)
        {
            return _context.Categories
                .Where(c => !c.ChildCategories.Any())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Category>> GetAllCategoriesFlatAsync(CancellationToken cancellationToken)
        {
            return await _context.Categories
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

    }
}