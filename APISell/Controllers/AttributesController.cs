using Application.DTOs.Attributes;
using Application.DTOs.Category;
using Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;

namespace APISell.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributesController : ControllerBase
    {
        private readonly IAttributesServices _attributesServices;
        public AttributesController(IAttributesServices attributesServices)
        {
            _attributesServices = attributesServices;
        }

        [HttpPost("add-attributes")]
        public async Task<IActionResult> AddAttributes([FromBody] AttributesDTO attributesDTO, CancellationToken cancellationToken)
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
    }
}
