using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Admin.Product
{
    public class ProductVariationDto
    {
        public Guid? ProductVariationId { get; set; }
        public string TypeName { get; set; }
        public decimal Price { get; set; }
        public int? Stock { get; set; }
    }
}
