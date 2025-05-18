using Domain.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class ProductDTO : BaseEntity
    {
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public string? PrivewImageUrl { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public int? TotalSold { get; set; }
        public string? Origin { get; set; }
        public string? Brand { get; set; }
        public List<string>? ProductImageUrls { get; set; }
        public string? UpdaterName { get; set; }
        public string? CreatorName { get; set; }
    }
}
