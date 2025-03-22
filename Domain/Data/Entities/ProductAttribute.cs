using Domain.Data.Base;

namespace Domain.Data.Entities
{
    public class ProductAttribute : BaseEntity
    {
        public int ProductAttributeId { get; set; }
        public Guid? ProductId { get; set; }
        public int? AttributeId { get; set; }
        public string? Value { get; set; }

        public Product Product { get; set; }
        public Attributes Attribute { get; set; }
    }
}