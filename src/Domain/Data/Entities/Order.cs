using Domain.Data.Base;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Data.Entities
{
    public class Order : BaseEntity
    {
        public Guid OrderId { get; set; }
        public string OrderCode { get; set; }
        public decimal ShippingFee { get; set; }
        public string RecipientName { get; set; }
        public string DeliveryAddress { get; set; }
        public string ReceiverPhone { get; set; }
        public Guid? CustomerId { get; set; }
        public int TotalOrderItems { get; set; }
        public decimal TotalAmount { get; set; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;
        public string? Note { get; set; }

        // Navigation property
        public Customer? Customer { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public DateTime? ConfirmedAt { get; set; }
        
        //QH Voucher
        public virtual ICollection<OrderVoucher>? OrderVouchers { get; set; } = new List<OrderVoucher>();

    }
}
