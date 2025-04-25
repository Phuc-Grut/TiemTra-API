using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Attributes
{
    public class AttributeDeleteResult
    {
        public int AttributeId { get; set; }
        public string AttributeName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
