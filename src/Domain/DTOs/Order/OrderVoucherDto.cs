namespace Domain.DTOs.Order
{
    public class OrderVoucherDto
    {
        public string VoucherCode { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }
        public DateTime UsedAt { get; set; }
    }
}