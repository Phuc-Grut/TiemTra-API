using Domain.Enum;

namespace Domain.DTOs.Product
{
    public class ProductFilterDto
    {
        public string? ProductCode { get; set; }
        public string? Keyword { get; set; }
        public ProductStatus? Status { get; set; }
        public string? SortBy { get; set; }
        public int? CategoryId { get; set; }
        public List<int>? CategoryIds { get; set; }
        public int? BrandId { get; set; }
    }
}