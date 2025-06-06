using Domain.Data.Base;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Admin.Product
{
    public class ProductDTO : BaseEntity
    {
        public Guid ProductId { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public string? PrivewImageUrl { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public int? TotalSold { get; set; }
        public string? Origin { get; set; }
        public string? Brand { get; set; }
        public string? Note { get; set; }
        public int? CategoryId { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public List<ProductVariationDto> ProductVariations { get; set; }
        public List<string>? ProductImageUrls { get; set; }
        public string? UpdaterName { get; set; }
        public string? CreatorName { get; set; }
    }
}
