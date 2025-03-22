using Domain.Data.Base;

namespace Domain.Data.Entities
{
    public class ProductVariations : BaseEntity
    {
        public int ProductVariationsId { get; set; }
        public Guid? ProductId { get; set; }
        public string VariationType { get; set; }
        public string VariationValue { get; set; }
        public Product Product { get; set; }

        //public Product? Product { get; set; }
        public ICollection<ProductVariationDetails> ProductVariationDetails { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; }
    }
}