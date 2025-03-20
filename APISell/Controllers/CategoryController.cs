using Application.DTOs;
using Application.DTOs.Category;
using Application.Interface;
using Domain.Data.Entities;
using Domain.DTOs.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;

namespace APISell.Controllers
{
    [Authorize]
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryServices _categoryServices;

        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        [HttpGet("get-paging-category")]
        public async Task<IActionResult> GetAllCategories( [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = null, CancellationToken cancellationToken = default)
        {
            try
            {
                pageNumber = pageNumber < 1 ? 1 : pageNumber;
                pageSize = pageSize < 1 ? 10 : pageSize;

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

                return Ok(category);
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
                    return NotFound();
                }
                return Ok(new
                {
                    success = true,
                    message = "Xóa thành công",
                });
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
                    return NotFound();
                }

                return Ok();
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
