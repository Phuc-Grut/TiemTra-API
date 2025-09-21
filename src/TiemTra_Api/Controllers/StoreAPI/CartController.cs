using Application.DTOs.Admin.Cart;
using Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TiemTra_Api.Controllers.StoreAPI
{
    [Authorize]
    [Route("api/store/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartServices _cartServices;

        public CartController(ICartServices cartServices)
        {
            _cartServices = cartServices;
        }

        [HttpPost("add-product-to-cart")]
        public async Task<IActionResult> AddProductToCart([FromBody] AddProductToCartRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null || request.ProductId == Guid.Empty)
            {
                return BadRequest("Yêu cầu không hợp lệ hoặc thiếu ProductId");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Không tìm thấy hoặc không hợp lệ thông tin người dùng");
            }

            var result = await _cartServices.AddProductToCart(userId, request.ProductId, request?.ProductVariationId, request.Quantity, cancellationToken);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpGet("view-cart")]
        public async Task<IActionResult> ViewCart(CancellationToken cancellationToken = default)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không tìm thấy thông tin người dùng");
            }

            var userId = Guid.Parse(userIdClaim.Value);

            var cart = await _cartServices.GetCartByUserId(userId, cancellationToken);

            if (cart == null)
            {
                return NotFound("Giỏ hàng không tồn tại");
            }

            return Ok(cart);
        }

        [HttpDelete("remove-cart-item")]
        public async Task<IActionResult> RemoveCartItem([FromQuery] Guid cartItemId, CancellationToken cancellationToken = default)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Không xác định được người dùng");
            }

            var result = await _cartServices.RemoveCartItemFromCartAsync(userId, cartItemId, cancellationToken);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPut("update-cart-item-quantity")]
        public async Task<IActionResult> UpdateCartItemQuantity([FromBody] AddProductToCartRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null || request.ProductId == Guid.Empty || request.Quantity <= 0)
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Không xác định được người dùng");
            }

            var result = await _cartServices.UpdateCartItemQuantityAsync(userId, request.ProductId, request.ProductVariationId, request.Quantity, cancellationToken);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpGet("get-total-quantity")]
        public async Task<IActionResult> GetTotalQuantity(CancellationToken cancellationToken)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Không xác định được người dùng.");

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
                return BadRequest("UserId không hợp lệ.");

            var totalQuantity = await _cartServices.GetTotalQuantityAsync(userId, cancellationToken);
            return Ok(totalQuantity);
        }
    }
}