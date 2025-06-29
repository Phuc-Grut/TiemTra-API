using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Admin.Brand
{
    public class BrandDTO
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string? Logo { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string? CreatorName { get; set; }
        public string? UpdaterName { get; set; }
    }
}
