using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs.Store.Voucher
{
    public class PublicVoucherDto
    {
        public Guid VoucherId { get; set; }
        public string VoucherCode { get; set; }
        public string VoucherName { get; set; }
        public string? Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime EndDate { get; set; }
        public int RemainingQuantity { get; set; }
        public bool IsAvailable => RemainingQuantity > 0 && EndDate > DateTime.UtcNow;
    }
}