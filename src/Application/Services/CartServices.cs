using Application.DTOs.Admin.Cart;
using Application.DTOs.Order;
using Application.Interface;
using Domain.Data.Entities;
using Domain.DTOs.Order;
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

            if (product.HasVariations == true)
            {
                if (productVariationId == Guid.Empty)
                {
                    return new ApiResponse(false, "Vui lòng chọn loại sản phẩm");
                }

                var variation = await _productVariationRepo.GetByIdAsync(productVariationId, cancellationToken);

                if (variation == null)
                {
                    return new ApiResponse(false, "Vui lòng chọn loại sản phẩm");
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

            cart.TotalItems = cart.CartItem.Sum(i => i.Quantity);
            cart.TotalPrice = cart.CartItem.Sum(i => i.Quantity * i.Price);
            await _cartRepository.UpdateCartAsync(cart, cancellationToken);

            return new ApiResponse(true, "Đã thêm sản phẩm vào giỏ hàng");
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
                ProductCode = ci.Product.ProductCode,
                ProductName = ci.Product?.ProductName ?? "",
                ProductVariationId = ci.ProductVariations?.ProductVariationId ?? Guid.Empty,
                ProductVariationName = ci.ProductVariations?.TypeName ?? "",
                PreviewImage = ci.Product?.PrivewImage ?? "",
                Price = ci.Price,
                Quantity = ci.Quantity
            }).ToList();

            return new CartDTO { Items = items, TotalPrice = cart.TotalPrice, TotalQuantity = cart.TotalItems };

        }

        public async Task<int> GetTotalQuantityAsync(Guid userId, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetCartByUserId(userId, cancellationToken);
            if (cart == null || cart.CartItem == null)
                return 0;

            return cart.CartItem.Sum(item => item.Quantity);
        }

        public async Task<ApiResponse> RemoveCartItemFromCartAsync(Guid userId, Guid cartItemId, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetCartByUserId(userId, cancellationToken);
            if (cart == null)
                return new ApiResponse(false, "Không tìm thấy giỏ hàng");

            var itemToRemove = cart.CartItem.FirstOrDefault(ci => ci.CartItemId == cartItemId);
            if (itemToRemove == null)
                return new ApiResponse(false, "Không tìm thấy sản phẩm trong giỏ hàng");

            await _cartRepository.RemoveCartItemAsync(itemToRemove, cancellationToken);

            cart.TotalItems = cart.CartItem.Where(i => i.CartItemId != cartItemId).Sum(i => i.Quantity);
            cart.TotalPrice = cart.CartItem.Where(i => i.CartItemId != cartItemId).Sum(i => i.Quantity * i.Price);

            await _cartRepository.UpdateCartAsync(cart, cancellationToken);

            return new ApiResponse(true, "Xóa sản phẩm thành công");
        }

        public async Task RemoveItemsFromCartAsync(Guid userId, IEnumerable<Guid> cartItemIds, CancellationToken cancellationToken)
        {
            foreach (var id in cartItemIds)
            {
                await _cartRepository.DeleteByIdAsync(userId, id, cancellationToken);
            }
            await _cartRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task<ApiResponse> UpdateCartItemQuantityAsync(Guid userId, Guid productId, Guid? productVariationId, int newQuantity, CancellationToken cancellationToken)
        {
            if (newQuantity <= 0)
                return new ApiResponse(false, "Số lượng phải lớn hơn 0");

            var cart = await _cartRepository.GetCartByUserId(userId, cancellationToken);
            if (cart == null)
                return new ApiResponse(false, "Không tìm thấy giỏ hàng");

            var itemToUpdate = cart.CartItem.FirstOrDefault(ci =>
                ci.ProductId == productId &&
                (
                    (productVariationId == Guid.Empty && (ci.ProductVariationId == null || ci.ProductVariationId == Guid.Empty)) ||
                    (ci.ProductVariationId == productVariationId)
                )
            );


            if (itemToUpdate == null)
                return new ApiResponse(false, "Không tìm thấy sản phẩm trong giỏ hàng");

            var product = await _productRepository.GetProductByIdAsync(productId, cancellationToken);
            if (product == null)
                return new ApiResponse(false, "Sản phẩm không tồn tại");

            if (product.HasVariations == true)
            {
                if (productVariationId == null || productVariationId == Guid.Empty)
                    return new ApiResponse(false, "Vui lòng chọn biến thể sản phẩm");

                var variation = await _productVariationRepo.GetByIdAsync(productVariationId.Value, cancellationToken);
               

                itemToUpdate.Quantity = newQuantity;
                itemToUpdate.Price = variation.Price;
            }
            else
            {
                itemToUpdate.Quantity = newQuantity;
                itemToUpdate.Price = product.Price ?? 0;
            }

            itemToUpdate.UpdatedAt = DateTime.UtcNow;

            cart.TotalItems = cart.CartItem.Sum(i => i.Quantity);
            cart.TotalPrice = cart.CartItem.Sum(i => i.Quantity * i.Price);

            await _cartRepository.UpdateCartItemAsync(cart, itemToUpdate, cancellationToken);
            await _cartRepository.UpdateCartAsync(cart, cancellationToken);

            return new ApiResponse(true, "Cập nhật số lượng sản phẩm thành công");
        }

        public async Task<List<Guid>> GetCartItemIdsMatchingOrderAsync(Guid userId, IEnumerable<CreateOrderItemDto> orderItems, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetCartByUserId(userId, cancellationToken);
            if (cart == null || cart.CartItem == null || !cart.CartItem.Any())
                return new List<Guid>();

            var cartItemIds = cart.CartItem
                .Where(ci => orderItems.Any(oi =>
                    oi.ProductId == ci.ProductId ||
                    oi.ProductVariationId == ci.ProductVariationId))
                .Select(ci => ci.CartItemId)
                .ToList();

            return cartItemIds;
        }
    }
}
