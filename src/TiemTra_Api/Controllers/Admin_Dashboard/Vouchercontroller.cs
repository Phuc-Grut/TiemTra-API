using Application.DTOs.Admin.Voucher;
using Application.Interface;
using Domain.DTOs.Admin.Voucher;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using System.Security.Claims;

namespace TiemTra_Api.Controllers.Admin_Dashboard
{
    [Route("api/admin/voucher")]
    //[Authorize(Roles = "Admin")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateVoucher([FromBody] CreateVoucherDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = User;

            var result = await _voucherService.CreateVoucherAsync(dto, user, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("unpublish/{voucherId}")]
        public async Task<IActionResult> UnpublishVoucher(
            Guid voucherId,
            [FromBody] string reason,
            CancellationToken cancellationToken)
        {
            if (voucherId == Guid.Empty)
                return BadRequest("Voucher ID không hợp lệ");

            if (string.IsNullOrWhiteSpace(reason))
                return BadRequest("Lý do không được để trống");

            var result = await _voucherService.UnpublishVoucherAsync(voucherId, User, cancellationToken);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("get-paging")]
        public async Task<IActionResult> GetPagingVouchers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] VoucherStatus? status = null,
            [FromQuery] string? keyword = null,
            CancellationToken cancellationToken = default)
        {
            var result = await _voucherService.GetPagedVouchersAsync(pageNumber, pageSize, status, keyword, cancellationToken);
            return Ok(result);
        }

        [HttpGet("get-by-id/{voucherId}")]
        public async Task<IActionResult> GetVoucherById(Guid voucherId, CancellationToken cancellationToken)
        {
            if (voucherId == Guid.Empty)
                return BadRequest("Voucher ID không hợp lệ");

            var result = await _voucherService.GetVoucherByIdAsync(voucherId, cancellationToken);

            if (result == null)
                return NotFound("Không tìm thấy voucher");

            return Ok(result);
        }

        [HttpPut("update-status/{voucherId}")]
        public async Task<IActionResult> UpdateVoucherStatus(
            Guid voucherId, 
            [FromBody] UpdateVoucherStatusDto dto, 
            CancellationToken cancellationToken)
        {
            if (voucherId == Guid.Empty)
                return BadRequest("Voucher ID không hợp lệ");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _voucherService.UpdateVoucherStatusAsync(voucherId, dto.Status, User, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("update/{voucherId}")]
        public async Task<IActionResult> UpdateVoucher(
            Guid voucherId, 
            [FromBody] CreateVoucherDto dto, 
            CancellationToken cancellationToken)
        {
            if (voucherId == Guid.Empty)
                return BadRequest("Voucher ID không hợp lệ");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _voucherService.UpdateVoucherAsync(voucherId, dto, User, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("generate-code")]
        public async Task<IActionResult> GenerateVoucherCode(CancellationToken cancellationToken)
        {
            var code = await _voucherService.GenerateVoucherCodeAsync(cancellationToken);
            return Ok(new { voucherCode = code });
        }
    }
}