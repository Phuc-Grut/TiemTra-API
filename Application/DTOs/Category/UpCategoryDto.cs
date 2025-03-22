using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Category
{
    public class UpCategoryDto
    {
        public string? CategoryName { get; set; }
        public int? ParentId { get; set; }
        public string? Description { get; set; }
    }
}
