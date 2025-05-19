using Application.DTOs;
using Application.DTOs.Product;
using Application.Interface;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TiemTra_Api.Controllers.Admin_Dashboard
{
    [Route("api/admin/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productService;
        private readonly IFileStorageService _fileStorageService;
        public ProductController(IProductServices productServices, IFileStorageService fileStorageService)
        {
            _productService = productServices;
            _fileStorageService = fileStorageService;
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = User;

            var result = await _productService.CreateProductAsync(dto, user, cancellationToken);

            if (!result)
            {
                return BadRequest(new { success = false, message = "Tạo sản phẩm thất bại" });
            }

            return Ok(new { success = true, message = "Tạo sản phẩm thành công" });

        }

        [HttpPost("product-image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadProductImage([FromForm] UploadFileDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("Không có file");

            var url = await _fileStorageService.UploadFileAsync(dto.File, "products/");
            return Ok(new { fileUrl = url });
        }


        [HttpGet("generate-product-code")]
        public async Task<IActionResult> GenerateProductCode(CancellationToken cancellationToken)
        {
            var productCode = await _productService.GenerateUniqueProductCodeAsync();
            return Ok(productCode);
        }

        [HttpGet("get-paging-products")]
        public async Task<IActionResult> GetPagingProducts(
            [FromQuery] ProductFilterRequest filterDto,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            CancellationToken cancellationToken = default)
        {
            var result = await _productService.GetPagingAsync(filterDto, pageNumber, pageSize, cancellationToken);
            return Ok(result);
        }

    }
}
