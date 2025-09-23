using Domain.Data.Entities;
using Shared.Common;

namespace Application.Interface
{
    public interface IInventoryService
    {
        Task UpdateStockAsync(IEnumerable<OrderItem> orderItems, CancellationToken cancellationToken);

        Task<ApiResponse> CheckStockAvailabilityAsync(IEnumerable<OrderItem> orderItems, CancellationToken cancellationToken);

        Task RestoreStockAsync(IEnumerable<OrderItem> orderItems, CancellationToken ct);
    }
}