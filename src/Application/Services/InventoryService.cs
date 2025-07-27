using Application.Interface;
using Domain.Data.Entities;
using Domain.Interface;
using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductVariationRepository _productVariationRepository;
        public InventoryService(IProductRepository productRepository, IProductVariationRepository productVariationRepository)
        {
            _productRepository = productRepository;
            _productVariationRepository = productVariationRepository;
        }

        public async Task<ApiResponse> CheckStockAvailabilityAsync(IEnumerable<OrderItem> orderItems, CancellationToken cancellationToken)
        {
            foreach (var item in orderItems)
            {
                if (item.ProductVariationId.HasValue)
                {
                    var variation = await _productVariationRepository.GetByIdAsync(item.ProductVariationId.Value, cancellationToken);
                    if (variation == null || variation.Stock < item.Quantity)
                    {
                        return new ApiResponse(false, $"Sản phẩm {variation?.Product?.ProductName} {variation.TypeName} không đủ số lượng");
                    }
                }
                else
                {
                    var product = await _productRepository.GetProductByIdAsync(item.ProductId, cancellationToken);
                    if (product == null || product.Stock < item.Quantity)
                    {
                        return new ApiResponse(false, $"Sản phẩm {product?.ProductName ?? "không rõ"} không đủ số lượng");
                    }
                }
            }

            return new ApiResponse(true, "Xác nhận thành công");
        }


        public async Task UpdateStockAsync(IEnumerable<OrderItem> orderItems, CancellationToken cancellationToken)
        {
            foreach (var item in orderItems) 
            {
                if (item.ProductVariationId.HasValue)
                {
                    var variation = await _productVariationRepository.GetByIdAsync(item.ProductVariationId.Value, cancellationToken);
                    if (variation == null) throw new Exception("Không tìm thấy biến thể sản phẩm");

                    variation.Stock -= item.Quantity;
                    variation.Product.Stock -= item.Quantity;
                    variation.Product.TotalSold += item.Quantity;
                }
                else
                {
                    var product = await _productRepository.GetProductByIdAsync(item.ProductId, cancellationToken);
                    if (product == null) throw new Exception("Không tìm thấy sản phẩm");

                    product.Stock -= item.Quantity;
                    product.TotalSold += item.Quantity;
                }
            }
        }
    }
}
