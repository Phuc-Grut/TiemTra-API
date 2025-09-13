using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Order
{
    public class ReturnCreateDto
    {
        public string Reason { get; set; } = default!;   // Lý do trả hàng (bắt buộc)
        public List<ReturnItemDto> Items { get; set; } = new();
    }

    public class ReturnItemDto
    {
        public Guid OrderItemId { get; set; }
        public int Quantity { get; set; }
    }
    public class OrderReturnInfo
    {
        public ReturnStatus Status { get; set; } = ReturnStatus.None;
        public string? Reason { get; set; }
        public DateTime? RequestedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public List<OrderReturnLine> Lines { get; set; } = new();
        public decimal? RefundAmount { get; set; }
        public string? RejectReason { get; set; }
    }

    public class OrderReturnLine
    {
        public Guid OrderItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } // snapshot từ OrderItem
        public decimal LineTotal => UnitPrice * Quantity;
    }

}
