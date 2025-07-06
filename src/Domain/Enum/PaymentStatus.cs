using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum PaymentStatus
    {
        Unpaid = 0, // Chưa thanh toán(COD)
        Paid = 1, // Đã thanh toán
        RefundRequested = 2, // Yêu cầu hoàn tiền (khách)
        Refunded = 3, // Đã hoàn tiền
    }
}
