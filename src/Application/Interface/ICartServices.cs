using Application.DTOs.Admin.Cart;
using Application.DTOs.Order;
using Domain.DTOs.Order;
using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ICartServices
    {
        Task<ApiResponse> AddProductToCart(Guid userId, Guid productId, Guid? productVariationId, int quantity, CancellationToken cancellationToken);
        Task<CartDTO> GetCartByUserId(Guid userId, CancellationToken cancellationToken);
        Task<ApiResponse> UpdateCartItemQuantityAsync(Guid userId, Guid productId, Guid? productVariationId, int newQuantity, CancellationToken cancellationToken);

        Task<ApiResponse> RemoveCartItemFromCartAsync(Guid userId, Guid cartItemId, CancellationToken cancellationToken);

        Task<int> GetTotalQuantityAsync(Guid userId, CancellationToken cancellationToken);

        Task RemoveItemsFromCartAsync(Guid userId, IEnumerable<Guid> cartItemIds, CancellationToken cancellationToken);

        Task<List<Guid>> GetCartItemIdsMatchingOrderAsync(Guid userId, IEnumerable<CreateOrderItemDto> orderItems, CancellationToken cancellationToken);

    }
}
