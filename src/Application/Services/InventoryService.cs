using Application.Interface;
using Domain.Data.Entities;
using Domain.Enum;
using Domain.Interface;
using Shared.Common;

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

        public async Task RestoreStockAsync(IEnumerable<OrderItem> orderItems, CancellationToken ct)
        {
            foreach (var item in orderItems)
            {
                if (item.ProductVariationId.HasValue)
                {
                    var variation = await _productVariationRepository.GetByIdAsync(item.ProductVariationId.Value, ct);
                    if (variation != null)
                    {
                        variation.Stock += item.Quantity;
                        variation.Product.Stock += item.Quantity;
                        variation.Product.TotalSold -= item.Quantity;

                        if (variation.Status == ProductVariationStatus.OutOfStock && item.Quantity > 0)
                        {
                            variation.Status = ProductVariationStatus.Active;
                        }
                    }
                }
                else
                {
                    var product = await _productRepository.GetProductByIdAsync(item.ProductId, ct);
                    if (product != null)
                    {
                        product.Stock += item.Quantity;
                        product.TotalSold -= item.Quantity;

                        if (product.ProductStatus == ProductStatus.OutOfStock && item.Quantity > 0)
                        {
                            product.ProductStatus = ProductStatus.Active;
                        }
                    }
                }
            }
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
                    if (variation == null)
                        throw new Exception("Không tìm thấy biến thể sản phẩm");

                    variation.Stock -= item.Quantity;
                    if (variation.Stock <= 0)
                    {
                        variation.Stock = 0;
                        variation.Status = ProductVariationStatus.OutOfStock;
                    }

                    variation.Product.Stock -= item.Quantity;
                    if (variation.Product.Stock <= 0)
                    {
                        variation.Product.Stock = 0;
                        variation.Product.ProductStatus = ProductStatus.OutOfStock;
                    }

                    variation.Product.TotalSold += item.Quantity;
                }
                else
                {
                    var product = await _productRepository.GetProductByIdAsync(item.ProductId, cancellationToken);
                    if (product == null)
                        throw new Exception("Không tìm thấy sản phẩm");

                    product.Stock -= item.Quantity;
                    if (product.Stock <= 0)
                    {
                        product.Stock = 0;
                        product.ProductStatus = ProductStatus.OutOfStock;
                    }

                    // Cập nhật tổng bán (chặn null/âm)
                    if (product.TotalSold < 0) product.TotalSold = 0;
                    product.TotalSold += item.Quantity;
                }
            }
        }
    }
}