using Application.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TiemTra_Api.Controllers.StoreAPI
{
    [Route("api/store/voucher")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [HttpGet("public")]
        public async Task<IActionResult> GetPublicVouchers(CancellationToken cancellationToken)
        {
            var result = await _voucherService.GetPublicVouchersAsync(cancellationToken);
            return Ok(result);
        }
    }
}