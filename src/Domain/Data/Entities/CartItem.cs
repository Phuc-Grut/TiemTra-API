using Domain.Data.Base;

namespace Domain.Data.Entities
{
    public class CartItem : BaseEntity
    {
        public Guid CartItemId { get; set; }
        public Guid CartId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? ProductVariationId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public Cart Cart { get; set; }
        public Product? Product { get; set; }
        public ProductVariations? ProductVariations { get; set; }
    }
}