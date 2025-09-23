using Application.DTOs.Order;
using Application.Interface;
using Domain.DTOs.Order;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using System.Security.Claims;

namespace TiemTra_Api.Controllers.Admin_Dashboard
{
    [Route("api/admin/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [HttpGet("get-paging-orders")]
        public async Task<IActionResult> GetPagingOrder([FromQuery] OrderFillterDto fillterDto, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, CancellationToken cancellationToken = default)
        {
            var result = await _orderServices.GetPagingOrder(fillterDto, pageNumber, pageSize, cancellationToken);
            return Ok(result);
        }

        [HttpPost("confirm-order/{orderId}")]
        public async Task<IActionResult> ConfirmOrder(Guid orderId, CancellationToken cancellationToken)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(new ApiResponse(false, "Không xác định được người dùng"));
            }

            var result = await _orderServices.ConfirmOrderAsync(orderId, userId, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("change-order-status/{orderId}")]
        public async Task<IActionResult> ChangeOrderStatus(Guid orderId, [FromBody] ChangeOrderStatusRequest request, CancellationToken cancellationToken)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(new ApiResponse(false, "Không xác định được người dùng"));
            }

            var result = await _orderServices.ChangeOrderStatus(orderId, request.NewStatus, userId, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("get-by-id/{orderId}")]
        public async Task<IActionResult> GetByIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(new ApiResponse(false, "Không xác định được người dùng"));
            }

            var result = await _orderServices.GetByIdAsync(orderId, cancellationToken);

            if (result == null)
            {
                return NotFound(new ApiResponse(false, "Đơn hàng không tồn tại"));
            }
            return Ok(result);
        }

        [HttpPost("{orderId:guid}/cancel")]
        public async Task<IActionResult> CancelOrder(Guid orderId, [FromBody] CancelOrderRequest body, CancellationToken ct)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var adminUserId))
            {
                return Unauthorized("Không xác định được người dùng");
            }
            var res = await _orderServices.CancelByAdminAsync(orderId, adminUserId, body?.Reason, ct);

            return Ok(res);
        }
    }
}