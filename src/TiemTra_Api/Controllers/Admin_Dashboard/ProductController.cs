using Application.DTOs.Product;
using Application.Interface;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TiemTra_Api.Controllers.Admin_Dashboard
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productService;
        public ProductController(IProductServices productServices)
        {
            _productService = productServices;
        }

        [HttpPost("create-product")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateProductDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = User; // lấy ClaimsPrincipal từ token nếu có

            var result = await _productService.CreateProductAsync(dto, user, cancellationToken);
            return Ok(result);
        }

        [HttpGet("generate-product-code")]
        public async Task<IActionResult> GenerateProductCode(CancellationToken cancellationToken)
        {
            var productCode = await _productService.GenerateUniqueProductCodeAsync();
            return Ok(productCode);
        }
    }
}
