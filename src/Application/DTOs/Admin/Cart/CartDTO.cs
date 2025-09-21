namespace Application.DTOs.Admin.Cart
{
    public class CartDTO
    {
        public List<CartItemDTO> Items { get; set; } = new();
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}