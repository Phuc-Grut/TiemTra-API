using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Category
{
    public class CategoryCheckResult
    {
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; } = string.Empty;
        public bool CanDelete { get; set; }
        public string? Message { get; set; } = string.Empty;
    }
}
