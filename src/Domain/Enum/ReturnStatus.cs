using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum ReturnStatus
    {
        None = 1,   // Chưa có yêu cầu trả hàng
        Requested = 10,   // Khách gửi yêu cầu trả hàng
        Approved = 1,   // Shop duyệt yêu cầu
        Rejected = 2,   // Shop từ chối
        InTransit = 3,   // Hàng đang trên đường trả về
        Completed = 4    // Hoàn tất quy trình (shop nhận hàng + hoàn tiền)
    }
}
