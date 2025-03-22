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
    public interface ICategoryAttributeService
    {
        Task<AddAttributeToCategoryDTO> AddAttributesToCategory(AddAttributeToCategoryDTO addDto, ClaimsPrincipal user,  CancellationToken cancellationToken);
        Task RemoveAttributesToCategory(int categoryId, int attributeId);
        Task<List<AddAttributeToCategoryDTO>> GetAttributesByCategory(int categoryId);
        //Task<bool> ExistsAsync(int categoryId, int attributeId);
    }
}
