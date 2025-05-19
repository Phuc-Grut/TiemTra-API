using Application.DTOs;
using Application.DTOs.Product;
using Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IProductServices
    {
        Task<bool> CreateProductAsync(CreateProductDto dto, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<string> GenerateUniqueProductCodeAsync();
        Task<PagedResult<ProductDTO>> GetPagingAsync(ProductFilterRequest filters, int pageNumber, int pageSize, CancellationToken cancellationToken);
    }
}
