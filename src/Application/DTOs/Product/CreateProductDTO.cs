using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class CreateProductDto
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PrivewImage { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public string Origin { get; set; }
        public bool HasVariations { get; set; }
        public int CategoryId { get; set; }
        public int? BrandId { get; set; }
        public List<ProductImageDto> ProductImages { get; set; } = new List<ProductImageDto>();
        public List<ProductAttributeDto> ProductAttributes { get; set; } = new List<ProductAttributeDto>();
        public List<ProductVariationDto> ProductVariations { get; set; } = new List<ProductVariationDto>();
    }
}

