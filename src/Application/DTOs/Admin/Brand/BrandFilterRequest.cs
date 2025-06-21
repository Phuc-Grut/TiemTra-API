using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Admin.Brand
{
    public class BrandFilterRequest
    {
       
        public string? Keyword { get; set; } 
        public bool? IsActive { get; set; }  
        public string? SortBy { get; set; }  
        public string? OriginCountry { get; set; } 
    }
}
