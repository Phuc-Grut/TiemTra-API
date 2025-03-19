using Application.DTOs.Category;
using Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ICategoryServices
    {
        Task<Category> AddCategory(CategoryDto categoryDto, ClaimsPrincipal user, CancellationToken cancellationToken);
        //Task DeleteCategory(int categoryId);
        Task<Category> GetCategoryById(int categoryId, CancellationToken cancellationToken);
        Task<IEnumerable<Category>> GetAllCategories(CancellationToken cancellationToken);
    }
}
