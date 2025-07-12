using Application.DTOs.Order;
using Domain.DTOs;
using Domain.DTOs.Order;
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
        Task<ApiResponse> CreateOrderAsync(CreateOrderRequest request, Guid? userId, CancellationToken cancellationToken);

        Task<PagedResult<OrderDto>> GetPagingOrder(OrderFillterDto fillterDto, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<string> GenerateUniqueOrderCodeAsync();
    }
}
