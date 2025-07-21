using Application.Interface;
using Domain.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TiemTra_Api.Controllers.StoreAPI
{
    [Route("api/store/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;

        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        [HttpGet("tree")] 
        public async Task<IActionResult> GetTreeCategory(CancellationToken cancellationToken)
        {
            try
            {
                var tree = await _categoryServices.GetCategoryTreeAsync(cancellationToken);
                return Ok(tree);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi khi lấy danh mục: {ex.Message}");
            }
        }
    }
}
