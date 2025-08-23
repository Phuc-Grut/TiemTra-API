
using Application.DTOs.Order;
using Application.Interface;
using Domain.Data.Entities;
using Domain.DTOs;
using Domain.DTOs.Order;
using Domain.Enum;
using Domain.Interface;
using Shared.Common;
using System.Text.RegularExpressions;

namespace Application.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductVariationRepository _productVariationRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICustomerService _customerService;
        private readonly IInventoryService _inventoryService;
        private readonly ICartServices _cartService;    

        public OrderServices(ICartServices cartServices, IInventoryService inventoryService, ICustomerService customerService, IOrderRepository orderRepository, IProductRepository productRepository, IProductVariationRepository productVariationRepository, IUserRepository userRepository, ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _productVariationRepository = productVariationRepository;
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _customerService = customerService;
            _inventoryService = inventoryService;
            _cartService = cartServices;
        }

        public async Task<string> GenerateUniqueOrderCodeAsync()
        {
            var random = new Random();
            string orderCode;
            bool exists;
            do
            {
                int ranDomNumber = random.Next(0, 9999);
                orderCode = $"DH{ranDomNumber:D3}";
                exists = await _orderRepository.OrderCodeExistsAsync(orderCode);
            }
            while (exists);
            return orderCode;
        }
      

        private async Task<OrderItem> CreateOrderItemAsync(CreateOrderItemDto itemDto, Guid orderId, CancellationToken cancellationToken)
        {
            decimal price;

            if (itemDto.ProductVariationId.HasValue && itemDto.ProductVariationId.Value != Guid.Empty)
            {
                var variation = await _productVariationRepository.GetByIdAsync(itemDto.ProductVariationId, cancellationToken);
                if (variation == null)
                    throw new Exception($"Vui lòng chọn biến thể sản phẩm");

                if (variation.Stock < itemDto.Quantity)
                {
                    throw new Exception($"Sản phẩm {variation.Product.ProductName} - {variation.TypeName}  không đủ số lượng trong kho");
                }

                if (variation.Status == ProductVariationStatus.Deleted || variation.Status == ProductVariationStatus.Inactive)
                    throw new Exception($"Sản phẩm {variation.Product.ProductName} - {variation.TypeName} đã ngừng bán, vui lòng xóa khỏi giỏ hàng");
                

                price = variation.Price;
            }
            else
            {
                var product = await _productRepository.GetProductByIdAsync(itemDto.ProductId, cancellationToken);
                if (product == null)
                    throw new Exception($"Không tìm thấy sản phẩm {itemDto.ProductId}");

                if (product.ProductStatus == ProductStatus.Deleted || product.ProductStatus == ProductStatus.Inactive || product.ProductStatus == ProductStatus.Draft)
                    throw new Exception($"Sản phẩm {product.ProductName} đã ngừng bán, vui lòng xóa khỏi giỏ hàng");

                if (product.Stock < itemDto.Quantity)
                    throw new Exception($"Sản phẩm {product.ProductName} không đủ số lượng trong kho");

                price = product.Price ?? 0;
            }

            return new OrderItem
            {
                OrderItemId = Guid.NewGuid(),
                OrderId = orderId,
                ProductId = itemDto.ProductId,
                ProductVariationId = itemDto.ProductVariationId,
                Quantity = itemDto.Quantity,
                UnitPrice = price,
                TotalPrice = price * itemDto.Quantity
            };
        }


        public async Task<ApiResponse> CreateOrderAsync(CreateOrderRequest request, Guid? userId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.RecipientName) ||
                string.IsNullOrWhiteSpace(request.RecipientPhone) ||
                string.IsNullOrWhiteSpace(request.RecipientAddress))
            {
                return new ApiResponse(false, "Vui lòng nhập đầy đủ địa chỉ nhận hàng");
            }

            if (!Regex.IsMatch(request.RecipientPhone, @"^(0|\+84)[0-9]{9,10}$"))
            {
                return new ApiResponse(false, "Số điện thoại không hợp lệ");
            }

            if (request.OrderItems == null || !request.OrderItems.Any())
                return new ApiResponse(false, "Vui lòng chọn sản phẩm");

            Guid customerId;
            try
            {
                customerId = await _customerService.GetOrCreateCustomerAsync(request, userId, cancellationToken);
            }
            catch (ArgumentException ex)
            {
                return new ApiResponse(false, ex.Message);
            }

            var newOrder = new Order
            {
                OrderId = Guid.NewGuid(),
                OrderCode = request.OrderCode,
                RecipientName = request.RecipientName,
                ReceiverPhone = request.RecipientPhone,
                DeliveryAddress = request.RecipientAddress,
                CustomerId = customerId,
                Note = request.Note,
                ShippingFee = request.ShippingFee,
                PaymentMethod = request.PaymentMethod,
                OrderItems = new List<OrderItem>(),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId ?? customerId
            };

            try
            {
                foreach (var itemDto in request.OrderItems)
                {
                    var orderItem = await CreateOrderItemAsync(itemDto, newOrder.OrderId, cancellationToken);
                    newOrder.OrderItems.Add(orderItem);
                }

                newOrder.TotalOrderItems = newOrder.OrderItems.Sum(ot => ot.Quantity);
                newOrder.TotalAmount = newOrder.OrderItems.Sum(ot => ot.TotalPrice);

                if (request.PaymentMethod == PaymentMethod.BankTransfer)
                {
                    newOrder.PaymentStatus = PaymentStatus.Paid;
                    newOrder.OrderStatus = OrderStatus.Confirmed;

                    await _inventoryService.UpdateStockAsync(newOrder.OrderItems, cancellationToken);
                }
                else
                {
                    newOrder.PaymentStatus = PaymentStatus.Unpaid;
                }



                await _orderRepository.AddOrderAsync(newOrder, cancellationToken);

                if (userId.HasValue && request.OrderItems != null && request.OrderItems.Any())
                {
                    var cartItemIds = await _cartService.GetCartItemIdsMatchingOrderAsync(userId.Value, request.OrderItems, cancellationToken);
                    await _cartService.RemoveItemsFromCartAsync(userId.Value, cartItemIds, cancellationToken);
                }

                return new ApiResponse(true, "Đặt đơn hàng thành công");
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, ex.Message);
            }
        }


        public async Task<PagedResult<OrderDto>> GetPagingOrder(OrderFillterDto filter, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await _orderRepository.GetPagedOrdersAsync(filter, pageNumber, pageSize, cancellationToken);
        }

        public async Task<ApiResponse> ConfirmOrderAsync(Guid orderId, Guid userId, CancellationToken cancellationToken)
        {
            if (orderId == Guid.Empty)
                return new ApiResponse(false, "Đã có lỗi xảy ra");

            var order = await _orderRepository.GetByIdWithItemsAsync(orderId, cancellationToken);
            if (order == null)
                return new ApiResponse(false, "Không tìm thấy đơn hàng");

            if (order.OrderStatus != OrderStatus.Pending)
                return new ApiResponse(false, "Đơn hàng không ở trạng thái chờ xác nhận");

            // Kiểm tra tồn kho qua InventoryService
            var checkResult = await _inventoryService.CheckStockAvailabilityAsync(order.OrderItems, cancellationToken);
            if (!checkResult.Success)
                return checkResult;

            // Trừ tồn kho qua InventoryService
            await _inventoryService.UpdateStockAsync(order.OrderItems, cancellationToken);

            if (order.PaymentMethod == PaymentMethod.BankTransfer)
            {
                order.PaymentStatus = PaymentStatus.Paid;
            }

            order.OrderStatus = OrderStatus.Confirmed;
            order.ConfirmedAt = DateTime.UtcNow;
            order.UpdatedBy = userId;

            await _orderRepository.SaveChangesAsync(cancellationToken);

            return new ApiResponse(true, "Xác nhận thành công");
        }


        public async Task<ApiResponse> ChangeOrderStatus(Guid orderId, OrderStatus newStatus, Guid userId, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
            if (order == null)
                return new ApiResponse(false, "Không tìm thấy đơn hàng");

            if (!OrderStatusValidator.CanChange(order.OrderStatus, newStatus))
            {
                var from = OrderStatusHelper.GetStatusDisplayName(order.OrderStatus);
                var to = OrderStatusHelper.GetStatusDisplayName(newStatus);

                return new ApiResponse(false, $"Không thể chuyển trạng thái từ {from} sang {to}");
            }

            order.OrderStatus = newStatus;
            order.UpdatedBy = userId;
            order.UpdatedAt = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order, cancellationToken);

            return new ApiResponse(true, "Chuyển trạng thái thành công");

        }

        public async Task<OrderDto> GetByIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Order ID không hợp lệ");

            var order = await _orderRepository.GetByIdWithItemsAsync(orderId, cancellationToken);

            if(order == null)
            {
                throw new Exception("Không tìm thấy đơn hàng");
            }

            var orderDto = new OrderDto
            {
                OrderCode = order.OrderCode,
                OrderStatus = order.OrderStatus,
                CreateAt = order.CreatedAt,
                ConfirmedAt = order?.ConfirmedAt,
                Note = order?.Note,
                TotalAmount = order.TotalAmount,
                TotalOrderItems = order.TotalOrderItems,

                CustomerCode = order.Customer.CustomerCode,
                CustomerName = order.Customer.CustomerName,
                ReceivertName = order.RecipientName,
                ReceiverPhone = order.ReceiverPhone,
                ReceiverAddress = order.DeliveryAddress,

                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductVariationId = oi.ProductVariationId ?? Guid.Empty,
                    ProductCode = oi.Product.ProductCode,
                    ProductName = oi.Product.ProductName,
                    PreviewImageUrl = oi.Product.PrivewImage,
                    VariationName = oi.ProductVariations?.TypeName,

                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity,
                    TotalPrice = oi.TotalPrice,
                }).ToList()
            };

            return orderDto;
        }
    }
}
