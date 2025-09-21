using Domain.Enum;

namespace Application.DTOs.Order
{
    public class ChangeOrderStatusRequest
    {
        public OrderStatus NewStatus { get; set; }
    }
}