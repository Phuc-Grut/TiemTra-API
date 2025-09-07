using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enum;

namespace Application.DTOs.Admin.Voucher
{
    public class VoucherDto
    {
         public Guid VoucherId { get; set; }
        public string VoucherCode { get; set; }
        public string VoucherName { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public int UsedQuantity { get; set; }
        public int RemainingQuantity => Quantity - UsedQuantity;
        public decimal DiscountPercentage { get; set; }
        public DateTime EndDate { get; set; }
        public VoucherStatus Status { get; set; }
        public string StatusDisplay => GetStatusDisplay();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatorName { get; set; }
        public string? UpdaterName { get; set; }

        private string GetStatusDisplay()
        {
            return Status switch
            {
                VoucherStatus.Pending => "Chờ phê duyệt",
                VoucherStatus.Publish => "Công khai",
                _ => "Không xác định"
            };
        }
    }
}