using Application.DTOs.Order;
using Domain.Data.Entities;
using Domain.DTOs;
using Domain.DTOs.Order;
using Domain.Enum;
using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IOrderServices
    {
        Task<ApiResponse> ConfirmOrderAsync(Guid orderId, Guid userId, CancellationToken cancellationToken);
        Task<PagedResult<OrderDto>> GetPagingOrder(OrderFillterDto fillterDto, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<OrderDto> GetByIdAsync(Guid orderId, CancellationToken cancellationToken);
        Task<ApiResponse> ChangeOrderStatus(Guid orderId, OrderStatus newStatus, Guid userId, CancellationToken cancellationToken);


        Task<ApiResponse> CreateOrderAsync(CreateOrderRequest request, Guid? userId, CancellationToken cancellationToken);
        Task<string> GenerateUniqueOrderCodeAsync();
    }
}
