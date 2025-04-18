using Application.DTOs.Attributes;
using Application.DTOs.Category;
using Application.Interface;
using Application.Services;
using Domain.DTOs.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APISell.Controllers.Admin_Dashboard
{
    [Authorize]
    [Route("api/admin/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;
        private readonly ICategoryAttributeService _categoryAttSv;

        public CategoryController(ICategoryServices categoryServices, ICategoryAttributeService categoryAttribute)
        {
            _categoryServices = categoryServices;
            _categoryAttSv = categoryAttribute;
        }

        [HttpGet("get-paging-categories")]
        public async Task<IActionResult> GetAllCategories( [FromQuery] string? keyword, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var filter = new CategoryFilterDto
            {
                Keyword = keyword
            };

            var result = await _categoryServices.GetAllCategories(filter, pageNumber, pageSize, cancellationToken);

            return Ok(result);
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

        [HttpPost("check-delete-by-ids")]
        public async Task<IActionResult> CheckDeleteCategories([FromBody] List<int> categoryIds, CancellationToken cancellationToken)
        {
            if (categoryIds == null || !categoryIds.Any())
                return BadRequest(new { success = false, message = "Danh sách danh mục không được để trống." });

            var results = await _categoryServices.CheckCanDeleteCategories(categoryIds, cancellationToken);
            var blockers = results.Where(r => !r.CanDelete).ToList();

            return Ok(new
            {
                success = true,
                canDeleteAll = !blockers.Any(),
                cannotDeleteCount = blockers.Count,
                blockers = blockers.Select(x => new
                {
                    x.CategoryName,
                    x.Message
                }),
                results
            });
        }

        [HttpDelete("delete-category-by-ids")]
        public async Task<IActionResult> DeleteMultipleCategories([FromBody] List<int> categoryIds, CancellationToken ct)
        {
            if (categoryIds == null || !categoryIds.Any())
                return BadRequest(new { success = false, message = "Danh sách danh mục không được để trống." });

            var results = await _categoryServices.DeleteCategoriesAsync(categoryIds, ct);

            var failed = results.Where(x => !x.IsDeleted).ToList();
            var success = results.Where(x => x.IsDeleted).ToList();

            return Ok(new
            {
                success = failed.Count == 0,
                deletedCount = success.Count,
                failedCount = failed.Count,
                results
            });
        }

        [HttpPut("update-category/{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] UpCategoryDto categoryDto, CancellationToken cancellationToken)
        {
            try
            {
                var user = HttpContext.User;

                var result = await _categoryServices.UpdateCategory(categoryId, categoryDto, user, cancellationToken);
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

        [HttpPost("get-by-id")]
        public async Task<IActionResult> GetCategoryById([FromBody] CategoryIdRequest2 rq, CancellationToken cancellationToken = default)
        {
            var result = await _categoryServices.GetCategoryById(
                rq.CategoryId,
                rq.PageNumber,
                rq.PageSize,
                cancellationToken
            );
            return Ok(result);
        }

        [HttpPost("set-attributes")]
        public async Task<IActionResult> SetAttributesForCategory([FromBody] SetAttributesForCategoryDTO dto, CancellationToken cancellationToken)
        {
            var user = HttpContext.User;
            await _categoryAttSv.SetAttributesForCategory(dto, user, cancellationToken);
            return Ok(new { success = true });
        }

        [HttpGet("{categoryId}/select-attributes")]
        public async Task<IActionResult> GetSelectedAttributes(int categoryId, CancellationToken cancellationToken)
        {
            var selected = await _categoryAttSv.GetSelectedAttributeIds(categoryId, cancellationToken);
            return Ok(selected);
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