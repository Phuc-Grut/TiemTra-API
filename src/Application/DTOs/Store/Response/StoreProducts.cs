using Application.DTOs.Admin.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Store.Response
{
    public class StoreProducts
    {
        public Guid ProductID { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public string? PrivewImageUrl { get; set; }
        public string? Price { get; set; }
        public List<ProductVariationDto> ProductVariations { get; set; }
        public List<string>? ProductImageUrls { get; set; }
    }
}
