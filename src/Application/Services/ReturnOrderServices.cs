using Application.DTOs.Order;
using Application.Interface;
using Domain.Data.Entities;
using Domain.Enum;
using Domain.Interface;
using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ReturnOrderServices : IReturnOrderServices
    {
        private readonly IOrderRepository _orderRepo;
        public ReturnOrderServices(IOrderRepository orderRepository)
        {
            _orderRepo = orderRepository;
        }

        public Task<ApiResponse> ApproveReturnAsync(Guid orderId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> CompleteReturnAsync(Guid orderId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> CreateReturnAsync(Guid orderId, ReturnCreateDto dto, CancellationToken cancellationToken)
        {
            var order = await _orderRepo.GetByIdWithItemsAsync(orderId, cancellationToken);
            if (order == null)
            {
                return new ApiResponse(false, "Không tìm thấy đơn hàng");
            }

            if (order.OrderStatus != OrderStatus.Delivered || order.PaymentStatus != PaymentStatus.Paid)
            {
                return new ApiResponse(false, "Đơn hiện tại không thể trả");
            }

            if (order.ReturnStatus == ReturnStatus.Approved || order.ReturnStatus == ReturnStatus.Requested || order.ReturnStatus == ReturnStatus.InTransit)
            {
                return new ApiResponse(false, "Đơn hàng đã có yêu cầu trả");
            }

            if (order.DeliveredAt.HasValue && (DateTime.UtcNow - order.DeliveredAt.Value).TotalHours > 24)
            {
                return new ApiResponse(false, "Đơn hàng đã giao quá 24h, không thể yêu cầu trả");
            }

            order.ReturnStatus = ReturnStatus.Requested;
            order.ReturnCreatedAt = DateTime.UtcNow;
            order.ReturnReason = dto.Reason;
            //order.ReturnRefundAmount = order.TotalAmount;

            await _orderRepo.UpdateAsync(order, cancellationToken);
            await _orderRepo.SaveChangesAsync(cancellationToken);

            return new ApiResponse(true, "Tạo yêu cầu trả hàng thành công");
        }

        public Task<ApiResponse> GetOrderAsync(Guid orderId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> MarkInTransitAsync(Guid orderId, string carrier, string trackingNumber, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> RejectReturnAsync(Guid orderId, string rejectReason, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
