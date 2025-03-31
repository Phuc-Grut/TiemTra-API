﻿using Application.DTOs.Attributes;
using Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APISell.Controllers
{
    [Authorize]
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
    }
}