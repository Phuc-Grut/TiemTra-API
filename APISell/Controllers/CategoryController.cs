using Application.DTOs.Category;
using Application.Interface;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using System.Security.Claims;

namespace APISell.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryServices _categoryServices;

        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        [HttpGet("get-all-category")]
        public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
        {
            try
            {
                var categories = await _categoryServices.GetAllCategories(cancellationToken);

                return Ok(new { success = true, message = "Lấy danh sách danh mục thành công", data = categories });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Lỗi server", error = ex.Message });
            }
        }


        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto categoryDto, CancellationToken cancellationToken)
        {
            

            try
            {
                // Lấy thông tin User từ HttpContext
                var user = HttpContext.User;

                var category = await _categoryServices.AddCategory(categoryDto, user, cancellationToken);

                if (category == null)
                {
                    return StatusCode(500, "Lỗi khi thêm danh mục.");
                }

                return Ok(new ApiResponse(true, "tạo danh mục thành công", categoryDto));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Bạn chưa đăng nhập.");
            }
        }

    }
}
