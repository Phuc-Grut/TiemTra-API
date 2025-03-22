using Application.DTOs;
using Application.DTOs.Category;
using Domain.DTOs.Category;
using System.Security.Claims;

namespace Application.Interface
{
    public interface ICategoryServices
    {
        Task<CategoryDto> AddCategory(UpCategoryDto categoryDto, ClaimsPrincipal user, CancellationToken cancellationToken);

        Task<object> GetCategoryById(int categoryId, CancellationToken cancellationToken);

        Task<PagedResult<CategoryDto>> GetAllCategories(CategoryFilterDto filters, int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<bool> DeleteCategory(int categoryId, CancellationToken cancellationToken);

        Task<bool> UpdateCategory(int categoryId, UpCategoryDto categoryDto, CancellationToken cancellationToken);

        //Task DeleteCategory(int categoryId);
        //Task<IEnumerable<CategoryDto>> FilterCategories(CategoryFilterDto filters, CancellationToken cancellationToken);
    }
}