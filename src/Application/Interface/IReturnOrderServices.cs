using Application.DTOs.Order;
using Domain.Data.Entities;
using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IReturnOrderServices
    {
        // Tạo yêu cầu trả hàng mới
        Task<ApiResponse> CreateReturnAsync(Guid orderId, ReturnCreateDto dto, CancellationToken cancellationToken);

        // Duyệt yêu cầu trả hàng
        Task<ApiResponse> ApproveReturnAsync(Guid orderId, CancellationToken cancellationToken);

        // Đánh dấu hàng đang trên đường trả về
        Task<ApiResponse> MarkInTransitAsync(Guid orderId, string carrier, string trackingNumber, CancellationToken cancellationToken);

        // Từ chối yêu cầu trả hàng
        Task<ApiResponse> RejectReturnAsync(Guid orderId, string rejectReason, CancellationToken cancellationToken);

        // Hoàn tất quy trình trả hàng (shop đã nhận + hoàn tiền)
        Task<ApiResponse> CompleteReturnAsync(Guid orderId, CancellationToken cancellationToken);

        // Lấy thông tin đơn (kèm trạng thái trả hàng)
        Task<ApiResponse> GetOrderAsync(Guid orderId, CancellationToken cancellationToken);
    }

}
