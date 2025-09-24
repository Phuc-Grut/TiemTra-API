using Application.DTOs.Order;
using Domain.DTOs;
using Domain.DTOs.Order;
using Domain.Enum;
using Shared.Common;

namespace Application.Interface
{
    public interface IOrderServices
    {
        Task<ApiResponse> ConfirmOrderAsync(Guid orderId, Guid userId, CancellationToken cancellationToken);

        Task<PagedResult<OrderDto>> GetPagingOrder(OrderFillterDto fillterDto, int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<OrderDto> GetByIdAsync(Guid orderId, CancellationToken cancellationToken);

        Task<ApiResponse> ChangeOrderStatus(Guid orderId, OrderStatus newStatus, Guid userId, CancellationToken cancellationToken);

        Task<ApiResponse> CancelByAdminAsync(Guid orderId, Guid adminUserId, string? reason, CancellationToken ct);

        Task<ApiResponse> CreateOrderAsync(CreateOrderRequest request, Guid? userId, CancellationToken cancellationToken);

        Task<string> GenerateUniqueOrderCodeAsync();

        Task<PagedResult<OrderDto>> GetByUserIDAsync(Guid UserID, OrderFillterDto fillterDto, int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<ApiResponse> CancelByCustomerAsync(Guid orderId, Guid customerUserId, string? reason, CancellationToken ct);

        Task<OrderDto> GetOrderWithVouchersAsync(Guid orderId, CancellationToken cancellationToken);
        Task<bool> CallBackUpdateStatusOrder(Guid orderId, OrderStatus newStatus, CancellationToken ct);
    }
}