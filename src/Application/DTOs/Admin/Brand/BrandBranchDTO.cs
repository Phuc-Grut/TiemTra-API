using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Admin.Brand
{
    public class BrandBranchDTO
    {
        public int? BrandLineId { get; set; }
        public string LineName { get; set; } = "";
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
    }
}
