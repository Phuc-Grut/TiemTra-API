using Application.DTOs.Admin.Product;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Store.Response
{
    public class StoreProducts
    {
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public string? PrivewImageUrl { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public int? TotalSold { get; set; }
        public int? CategoryId { get; set; }
        public List<ProductVariationDto> ProductVariations { get; set; }
        public List<string>? ProductImageUrls { get; set; }


        public bool HasVariations { get; set; }
        public string? CategoryName { get; set; }
        public ProductStatus ProductStatus { get; set; }


        public List<ProductAttributeDto> ProductAttributes { get; set; } = new();
    }
}
