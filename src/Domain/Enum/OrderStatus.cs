using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum OrderStatus
    {
        Pending = 10, // Đang chờ xử lý
        Confirmed = 1, // Đã xác nhận
        Shipping = 2, // Đang giao hàng
        Delivered = 3, // Đã giao hàng thành công

        DeliveryFailed = 4, // Giao hàng ko thành công
        CancelledByShop = 5, // Đã hủy
        CancelledByUser = 6, // Người dùng hủy đơn hàng
        Returned = 7 // Đã trả hàng
    }
}
