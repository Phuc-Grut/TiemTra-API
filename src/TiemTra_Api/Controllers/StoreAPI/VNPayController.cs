using Application.Interface;
using Domain.Data.Models.VNPAY;
using Domain.Enum;
using Domain.Enum.VNPAY;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Utilities.VNPAY;

namespace TiemTra_Api.Controllers.StoreAPI
{
    [ApiController]
    [Route("[controller]")]
    public class VnpayController : ControllerBase
    {
        private readonly IVNPayService _vnPayservice;
        private readonly IConfiguration _configuration;
        private readonly IOrderServices _orderService;

        public VnpayController(IVNPayService vnPayservice, IConfiguration configuration, IOrderServices orderService)
        {
            _vnPayservice = vnPayservice;
            _configuration = configuration;
            _orderService = orderService;
            _vnPayservice.Initialize(_configuration["Vnpay:TmnCode"], _configuration["Vnpay:HashSecret"], _configuration["Vnpay:BaseUrl"], _configuration["Vnpay:CallbackUrl"]);
        }

        /// <summary>
        /// Tạo url thanh toán
        /// </summary>
        /// <param name="money">Số tiền phải thanh toán</param>
        /// <param name="description">Mô tả giao dịch</param>
        /// <returns></returns>
        [HttpGet("api/CreatePaymentUrl")]
        public ActionResult<string> CreatePaymentUrl(double money, string description)
        {
            try
            {
                var ipAddress = NetworkHelper.GetIpAddress(HttpContext); // Lấy địa chỉ IP của thiết bị thực hiện giao dịch


                var request = new PaymentRequest
                {
                    PaymentId = DateTime.Now.Ticks,
                    Money = money,
                    Description = description,
                    IpAddress = ipAddress,
                    BankCode = BankCode.ANY, // Tùy chọn. Mặc định là tất cả phương thức giao dịch
                    CreatedDate = DateTime.Now, // Tùy chọn. Mặc định là thời điểm hiện tại
                    Currency = Currency.VND, // Tùy chọn. Mặc định là VND (Việt Nam đồng)
                    Language = DisplayLanguage.Vietnamese // Tùy chọn. Mặc định là tiếng Việt
                };

                var paymentUrl = _vnPayservice.GetPaymentUrl(request);

                return Created(paymentUrl, paymentUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Thực hiện hành động sau khi thanh toán. URL này cần được khai báo với VNPAY để API này hoạt đồng (ví dụ: http://localhost:1234/api/Vnpay/IpnAction)
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/IpnAction")]
        public async Task<IActionResult> IpnAction(CancellationToken ct)
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = _vnPayservice.GetPaymentResult(Request.Query);
                    if (paymentResult.IsSuccess)
                    {
                        Guid orderId = new Guid(paymentResult.Description);
                        await _orderService.CallBackUpdateStatusOrder(orderId, OrderStatus.Confirmed, ct);
                    }
                    else
                    {
                        // Thực hiện hành động nếu thanh toán thất bại tại đây. Ví dụ: Hủy đơn hàng.        
                        //return BadRequest("Thanh toán thất bại");
                        Guid orderId = new Guid(paymentResult.Description);
                        await _orderService.CallBackUpdateStatusOrder(orderId, OrderStatus.CancelledByShop, ct);
                    }
                    var redirectUrl = $"http://localhost:3000/";
                    return Redirect(redirectUrl);
                    
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return NotFound("Không tìm thấy thông tin thanh toán.");
        }

        /// <summary>
        /// Trả kết quả thanh toán về cho người dùng
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/Callback")]
        public ActionResult<PaymentResult> Callback()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = _vnPayservice.GetPaymentResult(Request.Query);

                    if (paymentResult.IsSuccess)
                    {
                        return Ok(paymentResult);
                    }

                    return BadRequest(paymentResult);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return NotFound("Không tìm thấy thông tin thanh toán.");
        }
    }
}
