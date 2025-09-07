using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Data.Base;
using Domain.Enum;

namespace Domain.Data.Entities
{
    public class Voucher : BaseEntity
    {
        public Guid VoucherId { get; set; } = Guid.NewGuid();
        public string VoucherCode { get; set; } // Mã voucher (unique)
        public string VoucherName { get; set; } // Tên voucher
        public string? Description { get; set; } // Mô tả
        public int Quantity { get; set; } // Số lượng
        public int UsedQuantity { get; set; } = 0; // Đã sử dụng
        public decimal DiscountPercentage { get; set; } // % giảm giá
        public DateTime EndDate { get; set; } // Ngày hết hạn
        public VoucherStatus Status { get; set; } = VoucherStatus.Pending;

        // Navigation properties - chỉ cần cho creator/updater
        public virtual User? Creator { get; set; }
        public virtual User? Updater { get; set; }

        // Quan hệ nhiều-nhiều với Order thông qua bảng trung gian
        public virtual ICollection<OrderVoucher>? OrderVouchers { get; set; } = new List<OrderVoucher>();
    }
}