using Application.DTOs.Attributes;
using Application.Interface;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APISell.Controllers.Admin_Dashboard
{
    [Authorize]
    [Route("api/admin/[controller]")]
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
    }
}