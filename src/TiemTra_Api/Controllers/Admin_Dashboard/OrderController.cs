using Application.DTOs.Order;
using Application.Interface;
using Application.Services;
using Domain.DTOs.Order;
using Domain.Enum;
using Microsoft.AspNetCore.Http;
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
    }
}
