using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class ProductFilterDto
    {
        public string? ProductCode { get; set; }
        public string? Keyword { get; set; }
    }
}
