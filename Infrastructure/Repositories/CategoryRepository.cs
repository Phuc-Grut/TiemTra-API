using Domain.Data.Entities;
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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<Category> AddCategory(Category category, CancellationToken cancellationToken)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllCategories(CancellationToken cancellationToken)
        {
            return await _context.Categories
                .Include(c => c.ChildCategories) // Lấy danh mục con ngay từ database
                .AsNoTracking() // Tăng hiệu suất, tránh lỗi vòng lặp EF
                .ToListAsync(cancellationToken);
        }

        public Task<Category> GetCategoryById(int categoryId, CancellationToken cancellationToken)
        {
            return _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId, cancellationToken);
        }
    }
}
