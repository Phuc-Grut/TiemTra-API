using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Admin.Brand
{
    public class BrandDeleteResult
    {
        public int BrandId { get; set; }
        public bool IsDeleted { get; set; }
        public string? Message { get; set; }
    }
}
