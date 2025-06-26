using Application.DTOs.Admin.Brand;
using Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TiemTra_Api.Controllers.Admin_Dashboard
{
    [Route("api/admin/brand")]
    //[Authorize(Roles = "Admin")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var brands = await _brandService.GetAllAsync(cancellationToken);
            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var brand = await _brandService.GetByIdAsync(id, cancellationToken);
            if (brand == null)
                return NotFound("Không tìm thấy thương hiệu.");

            return Ok(brand);
        }

        [HttpPost]
        public async Task<IActionResult> AddBrand([FromBody] CreateBrandDTO dto, CancellationToken cancellationToken)
        {
            var response = await _brandService.AddBrandAsync(dto, cancellationToken);
            if (!response.Success)
                return StatusCode(500, response.Message);

            return Ok(response.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBrandDTO dto, CancellationToken cancellationToken)
        {
            if (id != dto.BrandId)
                return BadRequest("ID không khớp.");

            try
            {
                var result = await _brandService.UpdateAsync(dto, cancellationToken);
                if (!result)
                    return NotFound("Không tìm thấy thương hiệu.");

                return Ok("Cập nhật thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Cập nhật thất bại: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _brandService.DeleteManyAsync(new List<int> { id }, cancellationToken);

                var deletedResult = result.FirstOrDefault();
                if (deletedResult == null || !deletedResult.IsDeleted)
                    return NotFound(deletedResult?.Message ?? "Không tìm thấy thương hiệu hoặc xoá thất bại.");

                return Ok(deletedResult.Message ?? "Xoá thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Xoá thất bại: {ex.Message}");
            }

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
    }
}




