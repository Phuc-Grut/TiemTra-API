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
        private readonly IProductRepository _product;
        private readonly IProductVariationRepository _productVariation;
        private readonly IProductImageRepository _producImage;
        private readonly IProductAttributeRepository _productAttribute;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "image-tiemtra";

        public ProductServices(IProductRepository product, IProductVariationRepository productVariation, IProductImageRepository producImage, IProductAttributeRepository productAttribute, BlobServiceClient blobServiceClient)
        {
            _product = product;
            _productVariation = productVariation;
            _producImage = producImage;
            _productAttribute = productAttribute;
            _blobServiceClient = blobServiceClient;
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
                    //PrivewImage = dto.PrivewImage,
                    Price = dto.Price,
                    Stock = dto.Stock,
                    Origin = dto.Origin,
                    HasVariations = dto.HasVariations,
                    CategoryId = dto.CategoryId,
                    BrandId = dto.BrandId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                };

                if (dto.PrivewImage != null && dto.PrivewImage.Length > 0)
                {
                    string blobUrl = await UploadImageToAzure(dto.PrivewImage);
                    product.PrivewImage = blobUrl;
                }

                await _product.AddAsync(product, cancellationToken);

                if (dto.ProductImages?.Any() == true)
                {
                    var imageUrls = new List<string>();
                    foreach (var productImage in dto.ProductImages)
                    {
                        if (productImage.ImageFile != null && productImage.ImageFile.Length > 0)
                        {
                            string blobUrl = await UploadImageToAzure(productImage.ImageFile);
                            imageUrls.Add(blobUrl);
                        }
                    }
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
                throw new Exception("Failed to create product", ex);
            }
        }

        private async Task<string> UploadImageToAzure(IFormFile file)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob); //truy cập công khai

            string originalName = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);
            string blobName = $"{originalName}{extension}";

            int suffix = 1;
            var blobClient = containerClient.GetBlobClient(blobName);
            while (await blobClient.ExistsAsync())
            {
                blobName = $"{originalName} ({suffix}){extension}";
                blobClient = containerClient.GetBlobClient(blobName);
                suffix++;
            }

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
            }

            return blobClient.Uri.ToString();
        }
    }
}
