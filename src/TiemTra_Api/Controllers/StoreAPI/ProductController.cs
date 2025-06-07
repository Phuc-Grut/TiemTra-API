using Application.DTOs.Admin.Product;
using Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TiemTra_Api.Controllers.StoreAPI
{
    [Route("api/store/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }


        [HttpGet("get-paging-products")]
        public async Task<IActionResult> StroreGetPagingProduct(
            [FromQuery] ProductFilterRequest filterDto,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            CancellationToken cancellationToken = default)
        {
            var result = await _productServices.StoreGetAllProductAsync(filterDto, pageNumber, pageSize, cancellationToken);
            return Ok(result);
        }

        [HttpGet("get-product-by-code/{productCode}")]
        public async Task<IActionResult> StoreGetProductByCode( string productCode, CancellationToken cancellationToken = default)
        {
            var result = await _productServices.StoreGetProductByCodeAsync(productCode, cancellationToken);
            if (result == null)
            {
                return NotFound("Có lỗ khi lấy dữ liệu");
            }
            return Ok(result);
        }
    }
}
