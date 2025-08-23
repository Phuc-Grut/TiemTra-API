using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Admin.Product
{
    public class BulkSoftDeleteRequest
    {
        public List<Guid> Ids { get; set; } = new();
    }
}
