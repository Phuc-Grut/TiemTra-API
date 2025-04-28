using Application.DTOs.Product;
using Application.Interface;
using Domain.Data.Entities;
using Domain.Interface;
using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepository _product;
        private readonly IProductVariationRepository _productVariation;
        private readonly IProductImageRepository _producImage;
        private readonly IProductAttributeRepository _productAttribute;

        public ProductServices(IProductRepository product, IProductVariationRepository productVariation, IProductImageRepository producImage, IProductAttributeRepository productAttribute)
        {
            _product = product;
            _productVariation = productVariation;
            _producImage = producImage;
            _productAttribute = productAttribute;
        }
        public async Task<CreateProductDto> CreateProductAsync(CreateProductDto dto, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            var userId = GetUserIdFromClaims.GetUserId(user);

            try
            {
                var product = new Product
                {
                    ProductId = Guid.NewGuid(),
                    ProductCode = dto.ProductCode,
                    ProductName = dto.ProductName,
                    PrivewImage = dto.PrivewImage,
                    Price = dto.Price,
                    Stock = dto.Stock,
                    Origin = dto.Origin,
                    HasVariations = dto.HasVariations,
                    CategoryId = dto.CategoryId,
                    BrandId = dto.BrandId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                };

                await _product.AddAsync(product, cancellationToken);

                if (dto.ProductImages?.Any() == true)
                {
                    var imageUrls = dto.ProductImages.Select(pi => pi.ImageUrl).ToList();
                    await _producImage.AddRangeAsync(product.ProductId, imageUrls, cancellationToken);
                }

                if (dto.ProductAttributes?.Any() == true)
                {
                    var productAttributes = dto.ProductAttributes.Select(attrDto => new ProductAttribute
                    {
                        ProductId = product.ProductId,
                        AttributeId = attrDto.AttributeId,
                        Value = attrDto.Value,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userId
                    }).ToList();

                    await _productAttribute.AddRangeAsync(productAttributes, cancellationToken);
                }

                if (dto.HasVariations && dto.ProductVariations?.Any() == true)
                {
                    var productVariations = dto.ProductVariations.Select(variationDto => new ProductVariations
                    {
                        ProductVariationId = Guid.NewGuid(),
                        ProductId = product.ProductId,
                        TypeName = variationDto.TypeName,
                        Price = variationDto.Price,
                        Stock = variationDto.Stock,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userId
                    }).ToList();

                    await _productVariation.AddRangeAsync(productVariations, cancellationToken);
                }

                dto.ProductCode = product.ProductCode;
                return dto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
