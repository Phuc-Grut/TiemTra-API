using Domain.Data.Base;

namespace Domain.Data.Entities
{
    public class OrderItem : BaseEntity
    {
        public Guid OrderItemId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Guid? ProductVariationId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public Order Order { get; set; }
        public Product? Product { get; set; }
        public ProductVariations? ProductVariations { get; set; }
    }
}