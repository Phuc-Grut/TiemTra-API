using Application.DTOs;
using Application.DTOs.Admin.Brand;
using Microsoft.AspNetCore.Http;
using Shared.Common;
using System.Security.Claims;

namespace Application.Interface
{
    public interface IBrandService
    {
        Task<ApiResponse> AddBrandAsync(CreateBrandDTO dto, ClaimsPrincipal user, CancellationToken cancellationToken);

        Task<bool> UpdateAsync(UpdateBrandDTO dto, CancellationToken cancellationToken);

        Task<List<BrandDeleteResult>> DeleteManyAsync(List<int> brandIds, CancellationToken cancellationToken);

        Task<IEnumerable<BrandDTO>> GetAllAsync(CancellationToken cancellationToken);

        Task<BrandDTO?> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<PagedResult<BrandDTO>> GetPagingAsync(BrandFilterDto filters, int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<int> GenerateUniqueBrandIdAsync(CancellationToken cancellationToken);

        Task<string> UploadBrandImageAsync(IFormFile file, CancellationToken cancellationToken);
    }
}