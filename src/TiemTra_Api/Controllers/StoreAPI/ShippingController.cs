using Application.DTOs.GHN;
using Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;

namespace TiemTra_Api.Controllers.StoreAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly IGhnService _ghnService;

        public ShippingController(IGhnService ghnService)
        {
            _ghnService = ghnService;
        }

        [HttpPost("calculate-fee")]
        public async Task<IActionResult> CalculateFee([FromBody] GhnCalculateFeeRequest req, CancellationToken ct)
        {
            try
            {
                var data = await _ghnService.CalculateFeeAsync(req, ct);
                // success
                return Ok(new ApiResponse(true, "OK", data));
            }
            catch (HttpRequestException ex)
            {
                // lỗi từ GHN (tham số/token/…)
                return BadRequest(new ApiResponse(false, $"GHN error: {ex.Message}"));
            }
            catch (System.Exception ex)
            {
                // lỗi phía server của bạn
                return StatusCode(500, new ApiResponse(false, $"Server error: {ex.Message}"));
            }
        }
    }
}
