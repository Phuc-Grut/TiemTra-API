using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Order
{
    public class CancelByAdminRequest
    {
        public string? Reason { get; set; }

        // Nếu sau này cần thêm trường thì dễ mở rộng
        public string? Note { get; set; }
        public bool NotifyCustomer { get; set; } = true;
    }
}
