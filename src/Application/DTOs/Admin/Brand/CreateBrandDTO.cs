using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Admin.Brand
{
    public class CreateBrandDTO
    {
        [Required]
        public string BrandName { get; set; }
        public string? Logo { get; set; }
        public string? Description { get; set; }
    }
}
