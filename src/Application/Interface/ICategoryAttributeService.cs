using Application.DTOs.Admin.Attributes;
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