using Domain.Enum;

namespace Domain.DTOs.Order
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public string OrderCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string ReceivertName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverPhone { get; set; }
        public decimal ItemsSubtotal { get; set; }
        public decimal TotalAmount { get; set; }
        public string Note { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public int TotalOrderItems { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public decimal ShippingFee { get; set; }
        public List<OrderVoucherDto>? AppliedVouchers { get; set; } = new();
    }
}