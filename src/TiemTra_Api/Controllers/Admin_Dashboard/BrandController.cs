using Application.DTOs;
using Application.DTOs.Admin.Brand;
using Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TiemTra_Api.Controllers.Admin_Dashboard
{
    [Route("api/admin/brand")]
    //[Authorize(Roles = "Admin")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private ClaimsPrincipal user;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var brand = await _brandService.GetByIdAsync(id, cancellationToken);
            if (brand == null)
                return NotFound("Không tìm thấy thương hiệu.");

            return Ok(brand);
        }

        //[HttpGet("all")]
        //public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        //{
        //    var brands = await _brandService.GetAllAsync(cancellationToken);
        //    return Ok(brands);
        //}

        [HttpPost("create-brand")]
        public async Task<IActionResult> AddBrand([FromBody] CreateBrandDTO dto, CancellationToken cancellationToken)
        {
            var response = await _brandService.AddBrandAsync(dto, cancellationToken);

            if (!response.Success)
                return StatusCode(500, new { success = false, message = response.Message });

            return Ok(new
            {
                success = true,
                message = response.Message,
            });
        }
          [HttpPut("update-brand")]
        public async Task<IActionResult> Update(int brandId, [FromBody] UpdateBrandDTO dto, CancellationToken cancellationToken)
        {
            if (brandId != dto.BrandId)
                return BadRequest("ID không khớp.");

            var result = await _brandService.UpdateAsync(dto, cancellationToken);
            if (!result)
                return NotFound("Không tìm thấy thương hiệu.");

            return Ok(new { success = true, message = "Cập nhật thành công." });
        }

        [HttpDelete("delete-brand-byid")]
        public async Task<IActionResult> Delete(int brandId, CancellationToken cancellationToken)
        {
            var result = await _brandService.DeleteManyAsync(new List<int> { brandId }, cancellationToken);

            var deletedResult = result.FirstOrDefault();
            if (deletedResult == null || !deletedResult.IsDeleted)
                return NotFound(new { success = false, message = deletedResult?.Message ?? "Xoá thất bại." });

            return Ok(new { success = true, message = deletedResult.Message });
        }

        [HttpGet("get-paging-brands")]
        public async Task<IActionResult> GetPagingBrands(
        [FromQuery] BrandFilterDto filters,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
        {
            var result = await _brandService.GetPagingAsync(filters, pageNumber, pageSize, cancellationToken);
            return Ok(result);
        }

        [HttpGet("generate-id")]
        public async Task<IActionResult> GenerateBrandId(CancellationToken cancellationToken)
        {
            var id = await _brandService.GenerateUniqueBrandIdAsync(cancellationToken);
            return Ok(id);
        }
        [HttpPost("brand-image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadBrandImage([FromForm] UploadFileDto dto, CancellationToken cancellationToken)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("Không có file");

            var url = await _brandService.UploadBrandImageAsync(dto.File, cancellationToken);
            return Ok(new { fileUrl = url });
        }

    }
}

