using Domain.Enum;

namespace Domain.DTOs.Order
{
    public class OrderFillterDto
    {
        public string? OrderCode { get; set; }
        public string? CustomerCode { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime? CreateAt { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        public string? SortBy { get; set; }
    }
}