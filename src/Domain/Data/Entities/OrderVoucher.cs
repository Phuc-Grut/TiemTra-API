using Domain.Data.Base;

namespace Domain.Data.Entities;

public class OrderVoucher : BaseEntity
{
    public Guid OrderVoucherId { get; set; } = Guid.NewGuid();
    public Guid OrderId { get; set; }
    public Guid VoucherId { get; set; }
    public string VoucherCode { get; set; } // Lưu mã voucher
    public decimal DiscountAmount { get; set; } // Số tiền được giảm thực tế
    public DateTime UsedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Order Order { get; set; }

    public virtual Voucher Voucher { get; set; }
}