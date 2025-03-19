using Domain.Data.Entities;
using Domain.DTOs.Category;
using Infrastructure.Database;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            await _context.SaveChangesAsync();
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


        //public async Task<IEnumerable<Category>> FilterCategories(CategoryFilterDto filters, CancellationToken cancellationToken)
        //{
        //    var query = _context.Categories.AsQueryable();

        //    if (!string.IsNullOrEmpty(filters.Keyword))
        //    {
        //        string keyword = filters.Keyword.ToLower();
        //        query = query.Where(c => c.CategoryName.ToLower().Contains(keyword));
        //    }

        //    if (filters.CategoryId.HasValue)
        //    {
        //        query = query.Where(c => c.CategoryId == filters.CategoryId);
        //    }

        //    return await query.ToListAsync(cancellationToken);
        //}
    }
}
