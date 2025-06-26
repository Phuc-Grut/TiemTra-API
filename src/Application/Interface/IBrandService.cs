using Application.DTOs.Admin.Brand;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Admin.Attributes;

namespace Application.Interface
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandDTO>> GetAllAsync(CancellationToken cancellationToken);
        Task<BrandDTO?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<BrandDTO> CreateAsync(CreateBrandDTO dto, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(UpdateBrandDTO dto, CancellationToken cancellationToken);
        Task<List<BrandDeleteResult>> DeleteManyAsync(List<int> brandIds, CancellationToken cancellationToken);

        //Task<object> GetPagedBrandsAsync(BrandFilterDto filter, CancellationToken cancellationToken);
        Task<PagedResult<BrandDTO>> GetAllPagedAsync(BrandFilterDto filters, int pageNumber, int pageSize, CancellationToken cancellationToken);





    }
}
