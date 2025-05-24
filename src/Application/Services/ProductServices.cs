using Application.DTOs;
using Application.DTOs.Product;
using Application.Interface;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Domain.Data.Entities;
using Domain.DTOs.Product;
using Domain.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        private readonly IUserRepository _userRepository;


        public ProductServices(IUserRepository userRepository ,IProductRepository product, IProductVariationRepository productVariation, IProductImageRepository producImage, IProductAttributeRepository productAttribute, BlobServiceClient blobServiceClient)
        {
            _productRepo = product;
            _productVariation = productVariation;
            _producImage = producImage;
            _productAttribute = productAttribute;
            _userRepository = userRepository;
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
                    ProductStatus = dto.ProductStatus,
                    Note = dto.Note,
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

        public async Task<PagedResult<ProductDTO>> GetPagingAsync(ProductFilterRequest filters, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var domainFilter = new ProductFilterDto
            {
                ProductCode = filters.ProductCode,
                Keyword = filters.Keyword,
                SortBy = filters.SortBy,
                CategoryId = filters.CategoryId,
                BrandId = filters.BrandId,
                Status = filters.Status
            };

            var query = _productRepo.GetFilteredProducts(domainFilter, cancellationToken);

            var totalItems = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var productsPage = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var userIds = productsPage
                .Select(p => p.CreatedBy)
                .Union(productsPage.Select(p => p.UpdatedBy))
                .Distinct()
                .ToList();

            var users = await _userRepository.GetUsersByIdsAsync(userIds, cancellationToken);
            var userDict = users.ToDictionary(u => u.UserId, u => u.FullName);

            var items = productsPage.Select(p =>
            {
                userDict.TryGetValue(p.CreatedBy, out var creatorName);
                userDict.TryGetValue(p.UpdatedBy, out var updaterName);

                return new ProductDTO
                {
                    ProductId = p.ProductId,
                    ProductCode = p.ProductCode,
                    PrivewImageUrl = p.PrivewImage,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    Origin = p.Origin,
                    TotalSold = p.TotalSold,
                    Brand = p.Brand?.BrandName,
                    Note = p.Note,
                    ProductStatus = p.ProductStatus,

                    ProductImageUrls = p.ProductImages != null
                        ? p.ProductImages.Select(pi => pi.ImageUrl).ToList()
                        : new List<string>(),

                    ProductVariations = p.ProductVariations != null
                        ? p.ProductVariations.Select(v => new ProductVariationDto
                        {
                            TypeName = v.TypeName,
                            Price = v.Price,
                            Stock = v.Stock
                        }).ToList()
                        : new List<ProductVariationDto>(),

                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    CreatorName = creatorName,
                    UpdaterName = updaterName,
                };
            }).ToList();

            return new PagedResult<ProductDTO>
            {
                Items = items,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize,
            };
        }
    }
}
