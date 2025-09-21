using Application.DTOs.Order;
using Application.Interface;
using Domain.DTOs.Order;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TiemTra_Api.Controllers.StoreAPI
{
    [Route("api/store/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
        {
            Guid? userId = null;

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var parsedId))
            {
                userId = parsedId;
            }

            var result = await _orderServices.CreateOrderAsync(request, userId, cancellationToken);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("generate-order-code")]
        public async Task<IActionResult> GenerateProductCode(CancellationToken cancellationToken)
        {
            var orderCode = await _orderServices.GenerateUniqueOrderCodeAsync();
            return Ok(orderCode);
        }

        [HttpGet("users/{userId}/orders")]
        public async Task<IActionResult> GetPagingOrderByUserId([FromRoute] Guid userId, [FromQuery] OrderFillterDto fillterDto, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, CancellationToken cancellationToken = default)
        {
            var result = await _orderServices.GetByUserIDAsync(userId, fillterDto, pageNumber, pageSize, cancellationToken);
            return Ok(result);
        }

        [HttpPost("{orderId:guid}/cancel")]
        public async Task<IActionResult> CancelOrder(Guid orderId, [FromBody] CancelOrderRequest request, CancellationToken ct)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Không tìm thấy hoặc không hợp lệ thông tin người dùng");
            }

            var res = await _orderServices.CancelByCustomerAsync(orderId, userId, request.Reason, ct);

            return Ok(res);
        }
    }
}