using Application.DTOs;
using Application.DTOs.Admin.Category;
using Application.DTOs.Store.Response;
using System.Security.Claims;

namespace Application.Interface
{
    public interface ICategoryServices
    {
        Task<CategoryDto> AddCategory(UpCategoryDto categoryDto, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<object> GetCategoryById(int categoryId, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<PagedResult<CategoryDto>> GetAllCategories(CategoryFilterDto filters, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<List<CategoryCheckResult>> CheckCanDeleteCategories(List<int> categoryIds, CancellationToken cancellationToken);
        Task<List<CategoryDeleteResult>> DeleteCategoriesAsync(List<int> categoryIds, CancellationToken cancellationToken);
        Task<bool> UpdateCategory(int categoryId, UpCategoryDto categoryDto, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<(bool CanDelete, string Message)> CheckIfCategoryCanBeDeleted(int categoryId, CancellationToken cancellationToken);
        Task<List<CategoryDto>> GetLeafCategoriesAsync(CancellationToken cancellationToken);
        Task<List<CategoryTreeDto>> GetCategoryTreeAsync(CancellationToken cancellation);

        //Task DeleteCategory(int categoryId);
        //Task<IEnumerable<CategoryDto>> FilterCategories(CategoryFilterDto filters, CancellationToken cancellationToken);
    }
}