using Application.DTOs;
using Application.DTOs.Category;
using Domain.Data.Entities;
using Domain.DTOs.Category;
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
        Task<Category> AddCategory(UpCategoryDto categoryDto, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<Category> GetCategoryById(int categoryId, CancellationToken cancellationToken);
        Task<PagedResult<CategoryDto>> GetAllCategories(CategoryFilterDto filters, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<bool> DeleteCategory(int categoryId, CancellationToken cancellationToken);
        Task<bool> UpdateCategory(int categoryId, UpCategoryDto categoryDto, CancellationToken cancellationToken);
        //Task DeleteCategory(int categoryId);
        //Task<IEnumerable<CategoryDto>> FilterCategories(CategoryFilterDto filters, CancellationToken cancellationToken);
    }
}
