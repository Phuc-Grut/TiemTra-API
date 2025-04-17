using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Attributes
{
    public class SetAttributesForCategoryDTO
    {
        public int CategoryId { get; set; }
        public List<int> AttributeIds { get; set; } = new();
    }
}
