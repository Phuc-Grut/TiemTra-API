
using Application.DTOs.Order;
using Application.Interface;
using Domain.Data.Entities;
using Domain.DTOs;
using Domain.DTOs.Order;
using Domain.Enum;
using Domain.Interface;
using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

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

        public OrderServices(ICustomerService customerService, IOrderRepository orderRepository, IProductRepository productRepository, IProductVariationRepository productVariationRepository, IUserRepository userRepository, ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _productVariationRepository = productVariationRepository;
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _customerService = customerService;
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

            if (itemDto.ProductVariationId.HasValue)
            {
                var variation = await _productVariationRepository.GetByIdAsync(itemDto.ProductVariationId, cancellationToken);
                if (variation == null)
                    throw new Exception($"Vui lòng chọn biến thể sản phẩm");

                price = variation.Price;
            }
            else
            {
                var product = await _productRepository.GetProductByIdAsync(itemDto.ProductId, cancellationToken);
                if (product == null)
                    throw new Exception($"Không tìm thấy sản phẩm {itemDto.ProductId}");

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
            if (request.OrderItems == null || !request.OrderItems.Any())
                return new ApiResponse(false, "Vui lòng chọn sản phẩm");

            //var orderCode = await GenerateUniqueOrderCodeAsync();

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

                PaymentMethod = request.PaymentMethod,

                OrderItems = new List<OrderItem>(),

                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId ?? customerId
            };

            decimal totalAmount = 0;

            foreach (var itemDto in request.OrderItems)
            {
                var orderItem = await CreateOrderItemAsync(itemDto, newOrder.OrderId, cancellationToken);
                newOrder.OrderItems.Add(orderItem);
                totalAmount = newOrder.OrderItems.Sum(ot => ot.TotalPrice);
            }
            newOrder.TotalOrderItems = newOrder.OrderItems.Sum(ot => ot.Quantity);
            newOrder.TotalAmount = totalAmount;

            await _orderRepository.AddOrderAsync(newOrder, cancellationToken);
            return new ApiResponse(true, "Đặt đơn hàng thành công");
        }

        public async Task<PagedResult<OrderDto>> GetPagingOrder(OrderFillterDto filter, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await _orderRepository.GetPagedOrdersAsync(filter, pageNumber, pageSize, cancellationToken);
        }

        public async Task<ApiResponse> ConfirmOrderAsync(Guid orderId, Guid userId, CancellationToken cancellationToken)
        {
            if (orderId == Guid.Empty)
                return new ApiResponse(false, "Đã có lỗi sảy ra");

            var order = await _orderRepository.GetByIdWithItemsAsync(orderId, cancellationToken);
            if (order == null)
                return new ApiResponse(false, "Không tìm thấy đơn hàng");

            if (order.OrderStatus != OrderStatus.Pending)
                return new ApiResponse(false, "Đơn hàng ko ở trạng thái chờ xác nhận");

            // Kiểm tra tồn kho
            foreach (var item in order.OrderItems)
            {
                if (item.ProductVariationId.HasValue)
                {
                    var variation = await _productVariationRepository.GetByIdAsync(item.ProductVariationId.Value, cancellationToken);
                    if (variation == null || variation.Stock < item.Quantity)
                        return new ApiResponse(false, $"Sản phẩm {variation?.Product?.ProductName} không đủ số lượng");
                }
                else
                {
                    var product = await _productRepository.GetProductByIdAsync(item.ProductId, cancellationToken);
                    if (product == null || product.Stock < item.Quantity)
                        return new ApiResponse(false, $"Sản phẩm {product.ProductName} không đủ số lượng");
                }
            }

            // Trừ tồn kho
            foreach (var item in order.OrderItems)
            {
                if (item.ProductVariationId.HasValue)
                {
                    var variation = await _productVariationRepository.GetByIdAsync(item.ProductVariationId.Value, cancellationToken);
                    variation.Stock -= item.Quantity;
                    variation.Product.Stock -= item.Quantity;
                    variation.Product.TotalSold += item.Quantity;
                }
                else
                {
                    var product = await _productRepository.GetProductByIdAsync(item.ProductId, cancellationToken);
                    product.Stock -= item.Quantity;
                    product.TotalSold += item.Quantity;
                }
            }

            // Cập nhật trạng thái đơn
            order.OrderStatus = OrderStatus.Confirmed;
            order.ConfirmedAt = DateTime.UtcNow;
            order.UpdatedBy = userId;

            // Save toàn bộ thay đổi 1 lần
            await _orderRepository.SaveChangesAsync(cancellationToken);

            return new ApiResponse(true, "Xác nhận thành công");
        }

    }
}
