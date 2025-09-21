namespace Application.DTOs.Admin.Cart
{
    public class AddProductToCartRequest
    {
        public Guid ProductId { get; set; }
        public Guid? ProductVariationId { get; set; }
        public int Quantity { get; set; }
    }
}