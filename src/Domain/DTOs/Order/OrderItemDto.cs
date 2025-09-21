namespace Domain.DTOs.Order
{
    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public Guid ProductVariationId { get; set; }
        public string ProductCode { get; set; } // Mã sản phẩm
        public string ProductName { get; set; }
        public string? PreviewImageUrl { get; set; }

        public string? VariationName { get; set; }  // ⬅️ Tên biến thể nếu có (VD: Size L, Màu đỏ)

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; } // Tổng giá trị của mặt hàng (Quantity * UnitPrice)
    }
}