using Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> AddCategory(Category category, CancellationToken cancellationToken);
        //Task DeleteCategory(int categoryId);
        Task<Category> GetCategoryById(int categoryId, CancellationToken cancellationToken);
        Task<IEnumerable<Category>> GetAllCategories( CancellationToken cancellationToken);
    }
}
