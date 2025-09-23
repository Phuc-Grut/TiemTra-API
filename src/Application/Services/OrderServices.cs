using Application.DTOs.Order;
using Application.DTOs.Store.Voucher;
using Application.Interface;
using Domain.Data.Entities;
using Domain.DTOs;
using Domain.DTOs.Order;
using Domain.Enum;
using Domain.Interface;
using Shared.Common;
using System.Text.Json;
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
        private readonly IVoucherRepository _voucherRepository;
        private readonly IVoucherService _voucherService;

        public OrderServices(ICartServices cartServices, IInventoryService inventoryService, ICustomerService customerService, IOrderRepository orderRepository, IProductRepository productRepository, IProductVariationRepository productVariationRepository, IUserRepository userRepository, ICartRepository cartRepository,
        IVoucherService voucherService, IVoucherRepository voucherRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _productVariationRepository = productVariationRepository;
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _voucherRepository = voucherRepository;
            _customerService = customerService;
            _inventoryService = inventoryService;
            _cartService = cartServices;
            _voucherService = voucherService;
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

                if (variation.Product.ProductStatus == ProductStatus.Deleted || variation.Product.ProductStatus == ProductStatus.Inactive || variation.Product.ProductStatus == ProductStatus.Draft)
                    throw new Exception($"Sản phẩm {variation.Product.ProductName} đã ngừng bán, vui lòng xóa khỏi giỏ hàng");

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
                ProductVariationId = itemDto.ProductVariationId.HasValue && itemDto.ProductVariationId.Value != Guid.Empty ? itemDto.ProductVariationId : null,
                Quantity = itemDto.Quantity,
                UnitPrice = price,
                TotalPrice = price * itemDto.Quantity
            };
        }

        public async Task<ApiResponse> CreateOrderAsync(CreateOrderRequest request, Guid? userId, CancellationToken cancellationToken)
        {
            Console.WriteLine("Request: " + JsonSerializer.Serialize(request));
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
                OrderVouchers = new List<OrderVoucher>(),
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

                // Xử lý voucher nếu có
                if (!string.IsNullOrEmpty(request.VoucherCode))
                {
                    newOrder.TotalOrderItems = newOrder.OrderItems.Sum(ot => ot.Quantity);
                    var voucherRequest = new ApplyVoucherRequest
                    {
                        VoucherCode = request.VoucherCode,
                        OrderTotal = newOrder.OrderItems.Sum(ot => ot.TotalPrice)
                    };

                    var voucherResult = await _voucherService.ApplyVoucherAsync(voucherRequest, cancellationToken);
                    if (!voucherResult.IsValid)
                    {
                        return new ApiResponse(false, voucherResult.Message);
                    }

                    // Tạo OrderVoucher
                    var orderVoucher = new OrderVoucher
                    {
                        OrderVoucherId = Guid.NewGuid(),
                        OrderId = newOrder.OrderId,
                        VoucherId = voucherResult.VoucherId.Value,
                        VoucherCode = voucherResult.VoucherCode,
                        DiscountAmount = voucherResult.DiscountAmount,
                        UsedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userId ?? customerId
                    };

                    newOrder.OrderVouchers.Add(orderVoucher);
                    newOrder.ItemsSubtotal = newOrder.OrderItems.Sum(ot => ot.TotalPrice);
                    newOrder.TotalAmount = newOrder.OrderItems.Sum(ot => ot.TotalPrice) - orderVoucher.DiscountAmount;

                    // Cập nhật UsedQuantity của voucher
                    await UpdateVoucherUsedQuantityAsync(voucherResult.VoucherId.Value, cancellationToken);
                }
                else
                {
                    newOrder.TotalOrderItems = newOrder.OrderItems.Sum(ot => ot.Quantity);
                    newOrder.ItemsSubtotal = newOrder.TotalAmount = newOrder.OrderItems.Sum(ot => ot.TotalPrice);
                }

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

                return new ApiResponse(true, "Đặt đơn hàng thành công", new { OrderId = newOrder.OrderId });
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, ex.Message);
            }
        }

        private async Task UpdateVoucherUsedQuantityAsync(Guid voucherId, CancellationToken cancellationToken)
        {
            var voucher = await _voucherRepository.GetByIdAsync(voucherId, cancellationToken);
            if (voucher != null)
            {
                voucher.UsedQuantity += 1;
                if(voucher.UsedQuantity == voucher.Quantity)
                {
                    voucher.Status = VoucherStatus.OutStock;
                }
                await _voucherRepository.UpdateAsync(voucher, cancellationToken);
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

        public async Task<ApiResponse> ChangeOrderStatus(Guid orderId, OrderStatus newStatus, Guid userId, CancellationToken ct)
        {
            var order = await _orderRepository.GetByIdAsync(orderId, ct);
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

            if (newStatus == OrderStatus.Delivered && order.PaymentStatus == PaymentStatus.Unpaid)
            {
                order.PaymentStatus = PaymentStatus.Paid;
                order.UpdatedAt = DateTime.UtcNow;
                order.ShippedAt = DateTime.UtcNow;

                order.Note = string.IsNullOrWhiteSpace(order.Note)
                    ? "[System] Auto mark Paid on Delivered (COD)"
                    : $"{order.Note}\n[System] Auto mark Paid on Delivered (COD)";
            }

            if (newStatus == OrderStatus.Delivered)
            {
                order.ShippedAt = DateTime.UtcNow;
                order.UpdatedAt = DateTime.UtcNow;
                order.UpdatedBy = userId;
            }

            if (newStatus == OrderStatus.Shipping)
            {
                order.ShippedAt = DateTime.UtcNow;
                order.UpdatedAt = DateTime.UtcNow;
                order.UpdatedBy = userId;
            }

            if (newStatus == OrderStatus.DeliveryFailed)
            {
                order.DeliveredAt = DateTime.UtcNow;
                order.UpdatedAt = DateTime.UtcNow;
                order.UpdatedBy = userId;
            }

            await _orderRepository.UpdateAsync(order, ct);
            return new ApiResponse(true, "Chuyển trạng thái thành công");
        }

        public async Task<OrderDto> GetByIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Order ID không hợp lệ");

            var order = await _orderRepository.GetByIdWithItemsAsync(orderId, cancellationToken);

            if (order == null)
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
                TotalAmount = order.TotalAmount, // sau khi giảm,
                ItemsSubtotal = order.ItemsSubtotal, // trước khi giảm
                TotalOrderItems = order.TotalOrderItems,

                CustomerCode = order.Customer.CustomerCode,
                CustomerName = order.Customer.CustomerName,
                ReceivertName = order.RecipientName,
                ReceiverPhone = order.ReceiverPhone,
                ReceiverAddress = order.DeliveryAddress,
                ShippingFee = order.ShippingFee,
                PaymentMethod = order.PaymentMethod,
                PaymentStatus = order.PaymentStatus,

                AppliedVouchers = order.OrderVouchers?.Select(ov => new OrderVoucherDto
                {
                    VoucherCode = ov.VoucherCode,
                    DiscountAmount = ov.DiscountAmount,
                    UsedAt = ov.UsedAt
                }).ToList() ?? new List<OrderVoucherDto>(),

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

        public async Task<PagedResult<OrderDto>> GetByUserIDAsync(Guid UserID, OrderFillterDto fillterDto, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await _orderRepository.GetByUserIDAsync(UserID, fillterDto, pageNumber, pageSize, cancellationToken);
        }

        public async Task<ApiResponse> CancelByCustomerAsync(Guid orderId, Guid customerUserId, string? reason, CancellationToken ct)
        {
            var order = await _orderRepository.GetByIdWithItemsAsync(orderId, ct);
            if (order == null)
                return new ApiResponse(false, "Không tìm thấy đơn hàng");

            if (!OrderStatusValidator.CanChange(order.OrderStatus, OrderStatus.CancelledByUser))
                return new ApiResponse(false, "Đơn hàng không thể hủy, vui lòng liên hệ shop");

            order.OrderStatus = OrderStatus.CancelledByUser;
            order.UpdatedBy = customerUserId;
            order.UpdatedAt = DateTime.UtcNow;
            order.CancelledAt = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(reason))
            {
                order.Note = $"[Khách hủy đơn]: {reason}";
            }

            await _orderRepository.UpdateAsync(order, ct);

            return new ApiResponse(true, "Hủy đơn thành công");
        }

        public async Task<ApiResponse> CancelByAdminAsync(Guid orderId, Guid adminUserId, string? reason, CancellationToken ct)
        {
            var order = await _orderRepository.GetByIdAsync(orderId, ct);
            if (order == null)
                return new ApiResponse(false, "Không tìm thấy đơn hàng");

            if (!OrderStatusValidator.CanChange(order.OrderStatus, OrderStatus.CancelledByShop))
                return new ApiResponse(false, "Đơn hàng ở trạng thái hiện tại không thể hủy");

            if (order.PaymentStatus == PaymentStatus.Paid)
            {
                order.PaymentStatus = PaymentStatus.Refunded;
            }

            if (order.OrderStatus == OrderStatus.Confirmed)
            {
                await _inventoryService.RestoreStockAsync(order.OrderItems, ct);
            }

            order.OrderStatus = OrderStatus.CancelledByShop;
            if (!string.IsNullOrWhiteSpace(reason))
            {
                order.Note = $"[Shop hủy đơn]: {reason}";
            }

            order.UpdatedBy = adminUserId;
            order.UpdatedAt = DateTime.UtcNow;
            order.CancelledAt = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order, ct);

            return new ApiResponse(true, "Hủy thành công");
        }

        public async Task<OrderDto> GetOrderWithVouchersAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderWithVouchersAsync(orderId, cancellationToken);
            if (order == null)
                throw new ArgumentException("Không tìm thấy đơn hàng");

            return new OrderDto
            {
                OrderId = order.OrderId,
                OrderCode = order.OrderCode,
                CustomerName = order.Customer?.CustomerName ?? "",
                CustomerCode = order.Customer?.CustomerCode ?? "",
                ReceivertName = order.RecipientName,
                ReceiverAddress = order.DeliveryAddress,
                ReceiverPhone = order.ReceiverPhone,
                TotalOrderItems = order.TotalOrderItems,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus,
                PaymentMethod = order.PaymentMethod,
                PaymentStatus = order.PaymentStatus,
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
                }).ToList(),
                Note = order.Note,
                CreateAt = order.CreatedAt,
                UpdateAt = order.UpdatedAt,
                ConfirmedAt = order.ConfirmedAt,
                ShippingFee = order.ShippingFee,
                //AppliedVouchers = order.OrderVouchers?.Select(ov => new Domain.DTOs.Order.OrderVoucherDto
                //{
                //    VoucherCode = ov.VoucherCode,
                //    DiscountAmount = ov.DiscountAmount,
                //    UsedAt = ov.UsedAt
                //}).ToList() ?? new List<Domain.DTOs.Order.OrderVoucherDto>()
            };
        }
    }
}