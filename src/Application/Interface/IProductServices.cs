using Application.DTOs;
using Application.DTOs.Admin.Product;
using Application.DTOs.Store.Response;
using System.Security.Claims;

namespace Application.Interface
{
    public interface IProductServices
    {
        Task<bool> CreateProductAsync(CreateProductDto dto, ClaimsPrincipal user, CancellationToken cancellationToken);

        Task<string> GenerateUniqueProductCodeAsync();

        Task<PagedResult<ProductDTO>> GetPagingAsync(ProductFilterRequest filters, int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<CreateProductDto> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken);

        Task<bool> UpdateProductAsync(Guid productId, ClaimsPrincipal user, CreateProductDto dto, CancellationToken cancellationToken);

        Task<int> SoftDeleteProductsAsync(IEnumerable<Guid> productIds, ClaimsPrincipal user, CancellationToken ct);

        Task<int> SoftDeleteByIdVarition(IEnumerable<Guid> ids, Guid updatedBy, CancellationToken ct);

        /// Store Product
        ///

        Task<PagedResult<StoreProducts>> StoreGetAllProductAsync(ProductFilterRequest filters, int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<StoreProducts> StoreGetProductByCodeAsync(string productCode, CancellationToken cancellationToken);
    }
}