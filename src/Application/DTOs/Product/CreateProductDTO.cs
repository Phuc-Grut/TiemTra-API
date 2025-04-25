using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class CreateProductDTO
    {
        public string? ProductCode { get; set; }
        public string ProductName { get; set; }
        public string? PrivewImage { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public string? Origin { get; set; }
        public string? Description { get; set; }
        public bool? HasVariations { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public List<string> ProductImages { get; set; } = new List<string>();
    }
}

