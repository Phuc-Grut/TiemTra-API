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
        Task<CreateProductDto> CreateProductAsync(CreateProductDto dto, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<string> GenerateUniqueProductCodeAsync();
    }
}
