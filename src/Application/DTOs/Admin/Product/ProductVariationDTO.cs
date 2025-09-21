using Domain.Enum;

namespace Application.DTOs.Admin.Product
{
    public class ProductVariationDto
    {
        public Guid? ProductVariationId { get; set; }
        public string TypeName { get; set; }
        public decimal Price { get; set; }
        public int? Stock { get; set; }
        public ProductVariationStatus Status { get; set; }
    }
}