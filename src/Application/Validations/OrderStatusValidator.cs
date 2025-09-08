using Domain.Enum;

namespace Application.Services
{
    public static class OrderStatusValidator
    {
        private static readonly Dictionary<OrderStatus, OrderStatus[]> ValidTransitions = new()
        {
            { OrderStatus.Pending, new[] { OrderStatus.Confirmed, OrderStatus.CancelledByUser, OrderStatus.CancelledByShop } },
            { OrderStatus.Confirmed, new[] { OrderStatus.Shipping, OrderStatus.CancelledByShop } },
            { OrderStatus.Shipping, new[] { OrderStatus.Delivered, OrderStatus.DeliveryFailed } },
            { OrderStatus.DeliveryFailed, new[] { OrderStatus.Shipping, OrderStatus.CancelledByShop } },
            { OrderStatus.CancelledByUser, Array.Empty<OrderStatus>() },
            { OrderStatus.CancelledByShop, Array.Empty<OrderStatus>() }
        };

        public static bool CanChange(OrderStatus current, OrderStatus next)
        {
            return ValidTransitions.TryGetValue(current, out var allowed)
                   && allowed.Contains(next);
        }
    }
}
