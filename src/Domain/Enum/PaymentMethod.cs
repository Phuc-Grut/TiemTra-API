using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum PaymentMethod
    {
        COD = 0,             // Thanh toán khi nhận hàng
        BankTransfer = 1,    // Chuyển khoản ngân hàng
    }
}
