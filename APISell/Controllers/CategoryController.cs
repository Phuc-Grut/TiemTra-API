using Application.DTOs.Category;
using Application.Interface;
using Domain.DTOs.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;

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

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllCategories( [FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? keyword = null, CancellationToken cancellationToken = default)
        {
            try
            {
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest(new { success = false, message = "PageNumber và PageSize phải lớn hơn 0." });
                }

                var filters = new CategoryFilterDto
                {
                    Keyword = keyword,
                    //ParentId = parentId,
                    //CategoryId = categoryId
                };

                var pagedResult = await _categoryServices.GetAllCategories(filters, pageNumber, pageSize, cancellationToken);

                return Ok(new
                {
                    success = true,
                    message = "Lấy danh sách danh mục thành công",
                    data = pagedResult.Items,
                    totalItems = pagedResult.TotalItems,
                    totalPages = pagedResult.TotalPages,
                    currentPage = pagedResult.CurrentPage,
                    pageSize = pagedResult.PageSize
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Đã xảy ra lỗi khi lấy danh mục",
                    error = ex.Message
                });
            }
        }

        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategory([FromBody] UpCategoryDto categoryDto, CancellationToken cancellationToken)
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
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpDelete("delete-category")]
        public async Task<IActionResult> DeleteCategory(int categoryId , CancellationToken cancellationToken)
        {
            try
            {
                var result = await _categoryServices.DeleteCategory(categoryId, cancellationToken);

                if (!result)
                {
                    return NotFound(new ApiResponse(false, "Danh mục không tồn tại."));
                }
                return Ok(new ApiResponse(true, "Xóa danh mục thành công."));
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

        [HttpPut("update-category")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpCategoryDto categoryDto, int categoryId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _categoryServices.UpdateCategory(categoryId, categoryDto, cancellationToken);
                if (!result)
                {
                    return NotFound(new ApiResponse(false, "Danh mục không tồn tại."));
                }

                return Ok(new ApiResponse(true, "Cập nhật danh mục thành công."));
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

        //[HttpGet("filter-category")]
        //public async Task<IActionResult> FilterCategories([FromQuery] CategoryFilterDto filters, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var categories = await _categoryServices.FilterCategories(filters, cancellationToken);
        //        return Ok(new ApiResponse(true, "Lọc anh mục thành công", categories));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new
        //        {
        //            success = false,
        //            error = ex.Message
        //        });
        //    }
        //}
    }
}
