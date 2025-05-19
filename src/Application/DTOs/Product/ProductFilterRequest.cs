using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class ProductFilterRequest
    {
        public string? ProductCode { get; set; }
        public string? Keyword { get; set; }
        public ProductStatus? Status { get; set; }
        public string? SortBy { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
    }
}
