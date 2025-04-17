using Application.DTOs.Attributes;
using Application.DTOs.Category;
using System.Security.Claims;

namespace Application.Interface
{
    public interface ICategoryAttributeService
    {
        Task SetAttributesForCategory(SetAttributesForCategoryDTO dto, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<List<int>> GetSelectedAttributeIds(int categoryId, CancellationToken cancellationToken);

        //Task<bool> ExistsAsync(int categoryId, int attributeId);
    }
}