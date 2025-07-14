using Domain.Enum;

namespace Application.Services
{
    public static class OrderStatusHelper
    {
        public static string GetStatusDisplayName(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => "Chờ xác nhận",
                OrderStatus.Confirmed => "Đã xác nhận",
                OrderStatus.Shipped => "Đang giao hàng",
                OrderStatus.Delivered => "Đã giao hàng",
                OrderStatus.DeliveryFailed => "Giao hàng thất bại",
                OrderStatus.CancelledByShop => "Shop đã hủy đơn",
                OrderStatus.CancelledByUser => "Khách đã hủy đơn",
                OrderStatus.Refunded => "Đã hoàn tiền",
                _ => "Không xác định"
            };
        }
    }
}
