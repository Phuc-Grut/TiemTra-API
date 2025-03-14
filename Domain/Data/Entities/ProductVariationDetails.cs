using Domain.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Data.Entities
{
    public class ProductVariationDetails : BaseEntity
    {
        public string ProductVariationDetailsId { get; set; }
        public int? ProductVariationsId { get; set; }
        public string? ProductCode { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public ProductVariations ProductVariation { get; set; }
    }
}
