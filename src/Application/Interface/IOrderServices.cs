using Application.DTOs.Order;
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
    }
}
