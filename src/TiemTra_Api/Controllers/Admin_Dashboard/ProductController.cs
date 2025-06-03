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
        private readonly IProductServices _productServices;
        private readonly IFileStorageService _fileStorageService;
        public ProductController(IProductServices productServices, IFileStorageService fileStorageService)
        {
            _productServices = productServices;
            _fileStorageService = fileStorageService;
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = User;

            var result = await _productServices.CreateProductAsync(dto, user, cancellationToken);

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
            var productCode = await _productServices.GenerateUniqueProductCodeAsync();
            return Ok(productCode);
        }

        [HttpGet("get-paging-products")]
        public async Task<IActionResult> GetPagingProducts(
            [FromQuery] ProductFilterRequest filterDto,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            CancellationToken cancellationToken = default)
        {
            var result = await _productServices.GetPagingAsync(filterDto, pageNumber, pageSize, cancellationToken);
            return Ok(result);
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetProductById( [FromQuery] Guid productId, CancellationToken cancellationToken)
        {
            if (productId == Guid.Empty)
                return BadRequest("Có lỗi khi lấy dữ liệu");

            var product = await _productServices.GetProductByIdAsync(productId, cancellationToken);
            if (product == null)
                return NotFound("Sản phẩm không tồn tại");

            return Ok(product);
        }


        [HttpPut("update-product/{productId}")]
        public async Task<IActionResult> Update(Guid productId, [FromBody] CreateProductDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _productServices.UpdateProductAsync(productId, User, dto, cancellationToken);

                if (result)
                    return Ok(new { success = true, message = "Cập nhật sản phẩm thành công" });

                return BadRequest(new { success = false, message = "Cập nhật sản phẩm thất bại" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
