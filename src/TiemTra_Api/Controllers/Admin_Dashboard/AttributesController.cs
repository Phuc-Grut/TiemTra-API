using Application.DTOs.Admin.Attributes;
using Application.Interface;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APISell.Controllers.Admin_Dashboard
{
    [Authorize]
    [Route("api/admin/attributes")]
    [ApiController]
    public class AttributesController : ControllerBase
    {
        private readonly IAttributesServices _attributesServices;

        public AttributesController(IAttributesServices attributesServices)
        {
            _attributesServices = attributesServices;
        }

        [HttpPost("add-attributes")]
        public async Task<IActionResult> AddAttributes([FromBody] AddAttributesDTO attributesDTO, CancellationToken cancellationToken)
        {
            try
            {
                // Lấy thông tin User từ HttpContext
                var user = HttpContext.User;

                var category = await _attributesServices.AddAttribute(attributesDTO, user, cancellationToken);

                if (category == null)
                {
                    return StatusCode(500, "Lỗi khi thêm thuộc tính.");
                }

                return Ok(attributesDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpGet("get-paging-attributes")]
        public async Task<IActionResult> GetAllAttributes( [FromQuery] string? keyword, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var filter = new AttributesFilterDTO
            {
                Keyword = keyword
            };

            var result = await _attributesServices.GetAllAttributes(filter, pageNumber, pageSize, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("delete-by-ids")]
        public async Task<IActionResult> DeleteAttributes([FromBody] List<int> attributeIds, CancellationToken cancellationToken)
        {
            if (attributeIds == null || !attributeIds.Any())
            {
                return BadRequest(new { success = false, message = "Danh sách thuộc tính không được để trống." });
            }

            var result = await _attributesServices.DeleteAttribute(attributeIds, cancellationToken);

            if (!result)
            {
                return StatusCode(500, new { success = false, message = "Xóa thuộc tính thất bại." });
            }

            return Ok(new { success = true, message = "Xóa thuộc tính thành công." });
        }

        [HttpPut("update-attributes/{attributeId}")]
        public async Task<IActionResult> UpdateAttribute(int attributeId, AddAttributesDTO attributesDTO, CancellationToken cancellationToken)
        {
            try
            {
                var user = HttpContext.User;
                var result = await _attributesServices.UpdateAttribute(attributeId, attributesDTO, user, cancellationToken);
                if (!result)
                {
                    return NotFound();
                }
                return Ok(new { message = "Cập nhật thuộc tính thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }
    }
}