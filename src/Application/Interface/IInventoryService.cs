using Domain.Data.Entities;
using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IInventoryService
    {
        Task UpdateStockAsync(IEnumerable<OrderItem> orderItems, CancellationToken cancellationToken);
        Task<ApiResponse> CheckStockAvailabilityAsync(IEnumerable<OrderItem> orderItems, CancellationToken cancellationToken);
        Task RestoreStockAsync(IEnumerable<OrderItem> orderItems, CancellationToken ct);
    }
}
