using Application.DTOs.Admin.Cart;
using Application.Interface;
using Domain.Data.Entities;
using Domain.Interface;
using Shared.Common;

namespace Application.Services
{
    public class CartServices : ICartServices
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductVariationRepository _productVariationRepo;

        public CartServices(ICartRepository cartRepository, IProductRepository productRepository, IProductVariationRepository productVariationRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _productVariationRepo = productVariationRepository;
        }

        public async Task<ApiResponse> AddProductToCart(Guid userId, Guid productId, Guid? productVariationId, int quantity, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetCartByUserId(userId, cancellationToken);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _cartRepository.AddCart(cart);
            }

            var product = await _productRepository.GetProductByIdAsync(productId, cancellationToken);
            if (product == null)
            {
                return new ApiResponse(false, "sản phẩm không tồn tại");
            }

            if (quantity <= 0)
            {
                return new ApiResponse(false, "Số lượng sản phẩm phải lớn hơn 0");
            }

            if (product.Stock < quantity)
            {
                return new ApiResponse(false, "Số lượng sản phẩm không đủ");
            }

            if (product.HasVariations == true)
            {
                if (productVariationId == Guid.Empty)
                {
                    return new ApiResponse(false, "Vui lòng chọn biến thể sản phẩm");
                }

                var variation = await _productVariationRepo.GetByIdAsync(productVariationId, cancellationToken);

                if (variation == null)
                {
                    return new ApiResponse(false, "Vui lòng chọn biến thể sản phẩm");
                }

                if (variation.Stock < quantity)
                {
                    return new ApiResponse(false, "Số lượng sản phẩm không đủ");
                }

                var existingItem = cart.CartItem.FirstOrDefault(ci => ci.ProductId == productId && ci.ProductVariationId == productVariationId);

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                    existingItem.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    var newItem = new CartItem
                    {
                        CartId = cart.CartId,
                        ProductId = productId,
                        ProductVariationId = productVariationId,
                        Quantity = quantity,
                        Price = variation.Price,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _cartRepository.AddCartItemAsync(cart, newItem, cancellationToken);
                }
            }

            else
            {
                if (product.Stock.HasValue && product.Stock.Value < quantity)
                    return new ApiResponse(false, "Sản phẩm trong kho không đủ.");

                var existingItem = cart.CartItem
                    .FirstOrDefault(ci => ci.ProductId == productId && ci.ProductVariationId == null);

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                    existingItem.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    var newItem = new CartItem
                    {
                        CartId = cart.CartId,
                        ProductId = productId,
                        Quantity = quantity,
                        Price = product.Price ?? 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _cartRepository.AddCartItemAsync(cart, newItem, cancellationToken);
                }
            }

            return new ApiResponse(true, "Thêm sản phẩm vào giỏ hàng thành công");
        }

        public  async Task<CartDTO> GetCartByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetCartByUserId(userId, cancellationToken);
            if (cart == null)
            {
                return new CartDTO();
            }

            var items = cart.CartItem.Select(ci => new CartItemDTO
            {
                CartItemId = ci.CartItemId,
                ProductId = ci.ProductId ?? Guid.Empty,
                ProductName = ci.Product?.ProductName ?? "",
                ProductVariationName = ci.ProductVariations?.TypeName ?? "",
                PreviewImage = ci.Product?.PrivewImage ?? "",
                Price = ci.Price,
                Quantity = ci.Quantity
            }).ToList();

            return new CartDTO { Items = items };

        }
    }
}
