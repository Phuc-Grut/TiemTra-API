using Application.DTOs;
using Application.DTOs.Product;
using Application.Interface;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Domain.Data.Entities;
using Domain.Interface;
using Microsoft.AspNetCore.Http;
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
        private readonly IProductRepository _productRepo;
        private readonly IProductVariationRepository _productVariation;
        private readonly IProductImageRepository _producImage;
        private readonly IProductAttributeRepository _productAttribute;
        
        
        public ProductServices(IProductRepository product, IProductVariationRepository productVariation, IProductImageRepository producImage, IProductAttributeRepository productAttribute, BlobServiceClient blobServiceClient)
        {
            _productRepo = product;
            _productVariation = productVariation;
            _producImage = producImage;
            _productAttribute = productAttribute;        
        }
        public async Task<bool> CreateProductAsync(CreateProductDto dto, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            var userId = GetUserIdFromClaims.GetUserId(user);

            try
            {
                if (await _productRepo.ProductCodeExistsAsync(dto.ProductCode))
                {
                    throw new Exception("Mã sản phẩm đã tồn tại");
                }

                var product = new Product
                {
                    ProductId = Guid.NewGuid(),
                    ProductCode = dto.ProductCode,
                    ProductName = dto.ProductName,
                    PrivewImage = dto.PrivewImageUrl,
                    Price = dto.Price,
                    Stock = dto.Stock,
                    Origin = dto.Origin,
                    HasVariations = dto.HasVariations,
                    CategoryId = dto.CategoryId,
                    BrandId = dto.BrandId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                };


                await _productRepo.AddAsync(product, cancellationToken);

                if (dto.ProductImageUrls?.Any() == true)
                {
                    await _producImage.AddRangeAsync(product.ProductId, dto.ProductImageUrls, cancellationToken);
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

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Tạo sản phẩm mới thất bại", ex);
            }
        }

        public async Task<string> GenerateUniqueProductCodeAsync()
        {
            var random = new Random();
            string productCode;
            bool exists;
            do
            {
                int ranDomNumber = random.Next(0, 1000);
                productCode = $"SP{ranDomNumber:D3}";
                exists = await _productRepo.ProductCodeExistsAsync(productCode);
            }
            while (exists);
            return productCode;
        }

        public Task<PagedResult<ProductDTO>> GetPagingAsync(ProductFilterDto filters, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
