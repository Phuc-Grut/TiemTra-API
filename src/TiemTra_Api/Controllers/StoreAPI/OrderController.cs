using Application.DTOs.Order;
using Application.Interface;
using Application.Services.Admin;
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
    }
}
