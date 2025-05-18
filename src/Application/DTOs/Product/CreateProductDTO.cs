using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class CreateProductDto
    {
        public string ProductCode { get; set; } = default!;
        public string ProductName { get; set; } = default!;
        public string PrivewImageUrl { get; set; } = default!;
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public string Origin { get; set; } = string.Empty;
        public bool HasVariations { get; set; }
        public int CategoryId { get; set; }
        public int? BrandId { get; set; }

        public List<string> ProductImageUrls { get; set; } = new();

        public List<ProductAttributeDto> ProductAttributes { get; set; } = new();
        public List<ProductVariationDto> ProductVariations { get; set; } = new();
    }

}

