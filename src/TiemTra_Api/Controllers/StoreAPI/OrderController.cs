using Application.DTOs.Order;
using Application.Interface;
using Application.Services;
using Domain.Data.Models.VNPAY;
using Domain.DTOs.Order;
using Domain.Enum.VNPAY;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using Shared.Utilities.VNPAY;
using System.Security.Claims;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Domain.Data.Entities;

namespace TiemTra_Api.Controllers.StoreAPI
{
    [Route("api/store/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        private readonly IVNPayService _vnPayservice;
        private readonly IConfiguration _configuration;

        public OrderController(IOrderServices orderServices, IVNPayService vnPayservice, IConfiguration configuration)
        {
            _orderServices = orderServices;
            _vnPayservice = vnPayservice;
            _configuration = configuration;
            _vnPayservice.Initialize(_configuration["Vnpay:TmnCode"], _configuration["Vnpay:HashSecret"], _configuration["Vnpay:BaseUrl"], _configuration["Vnpay:CallbackUrl"]);
            _configuration = configuration;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
        {
            Guid? userId = null;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var parsedId))
                userId = parsedId;

            var result = await _orderServices.CreateOrderAsync(request, userId, cancellationToken);

            if (!result.Success || result.Data is null)
                return BadRequest(result);

            var dataEl = JsonDocument.Parse(JsonSerializer.Serialize(result.Data)).RootElement;
            var orderId = Guid.Parse(dataEl.GetProperty("OrderId").GetString()!);
            var totalAmount = dataEl.GetProperty("TotalAmount").GetDecimal();
            var shippingFee = dataEl.GetProperty("ShippingFee").GetDecimal();


            if (request.PaymentMethod == Domain.Enum.PaymentMethod.COD)
            {
                result.Data = new { OrderId = orderId, TotalAmount = totalAmount, Navigate = false, PaymentUrl = "" };
                return Ok(result);
            }

            if (request.PaymentMethod == Domain.Enum.PaymentMethod.BankTransfer)
            {
                var ipAddress = NetworkHelper.GetIpAddress(HttpContext);

                var payload = new PaymentRequest
                {
                    PaymentId = DateTime.Now.Ticks,
                    Money = (double)totalAmount + (double)shippingFee,
                    Description = orderId.ToString(),
                    IpAddress = ipAddress,
                    BankCode = BankCode.ANY,
                    CreatedDate = DateTime.Now, // dùng UTC
                    Currency = Currency.VND,
                    Language = DisplayLanguage.Vietnamese
                };

                var paymentUrl = _vnPayservice.GetPaymentUrl(payload);
                result.Data = new { OrderId = orderId, TotalAmount = totalAmount, Navigate = true, PaymentUrl = paymentUrl };
                return Ok(result);
            }

            return Ok(result);
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
    public class OrderResponse
    {
        public Guid OrderId { get; set; } // Điều chỉnh kiểu dữ liệu nếu cần
        public decimal TotalAmount { get; set; } // Điều chỉnh kiểu dữ liệu nếu cần
    }
}