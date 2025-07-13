using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public decimal TotalAmount { get; set; }
        public string Note { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        public List<OrderItemDto> OrderItems { get; set; } = new();
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
