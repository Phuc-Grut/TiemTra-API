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
    }
}
