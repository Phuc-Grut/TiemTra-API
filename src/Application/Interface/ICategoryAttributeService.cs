using Application.DTOs.Category;
using System.Security.Claims;

namespace Application.Interface
{
    public interface ICategoryAttributeService
    {
        Task<AddAttributeToCategoryDTO> AddAttributesToCategory(AddAttributeToCategoryDTO addDto, ClaimsPrincipal user, CancellationToken cancellationToken);

        Task RemoveAttributesToCategory(int categoryId, int attributeId);

        //Task<bool> ExistsAsync(int categoryId, int attributeId);
    }
}