using Application.DTOs.Order;
using Application.Interface;
using Domain.Data.Entities;
using Domain.Enum;
using Domain.Interface;
using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private async Task<string> GenerateUniqueOrderCodeAsync()
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
                Price = price
            };
        }


        public async Task<ApiResponse> CreateOrderAsync(CreateOrderRequest request, Guid? userId, CancellationToken cancellationToken)
        {
            if (request.OrderItems == null || !request.OrderItems.Any())
                return new ApiResponse(false, "Vui lòng chọn sản phẩm");

            var orderCode = await GenerateUniqueOrderCodeAsync();

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
                OrderCode = orderCode,
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
                totalAmount += orderItem.Price * orderItem.Quantity;
            }
            newOrder.TotalOrderItems = newOrder.OrderItems.Sum(ot => ot.Quantity);
            newOrder.TotalAmount = totalAmount;

            await _orderRepository.AddOrderAsync(newOrder, cancellationToken);
            return new ApiResponse(true, "Đặt đơn hàng thành công");
        }
        
    }
}
