using Domain.Enum;

namespace Application.DTOs.Admin.Product
{
    public class ProductFilterRequest
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