using Application.DTOs;
using Application.DTOs.Admin.Category;
using Application.DTOs.Admin.Product;
using Application.DTOs.Store.Response;
using Application.Interface;
using Azure.Storage.Blobs;
using Domain.Data.Entities;
using Domain.DTOs.Product;
using Domain.Enum;
using Domain.Interface;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using System.Security.Claims;

namespace Application.Services.Admin
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepository _productRepo;
        private readonly IProductVariationRepository _productVariation;
        private readonly IProductImageRepository _producImage;
        private readonly IProductAttributeRepository _productAttribute;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _category;

        public ProductServices(ICategoryRepository category, IUserRepository userRepository, IProductRepository product, IProductVariationRepository productVariation, IProductImageRepository producImage, IProductAttributeRepository productAttribute, BlobServiceClient blobServiceClient)
        {
            _productRepo = product;
            _productVariation = productVariation;
            _producImage = producImage;
            _productAttribute = productAttribute;
            _userRepository = userRepository;
            _category = category;
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

                var hasVariations = dto.ProductVariations?.Any() == true;

                if (hasVariations)
                {
                    var productVariations = dto.ProductVariations.Select(variationDto => new ProductVariations
                    {
                        ProductVariationId = Guid.NewGuid(),
                        ProductId = product.ProductId,
                        TypeName = variationDto.TypeName,
                        Price = variationDto.Price,
                        Stock = variationDto.Stock,
                        Status = variationDto.Stock == 0
                            ? ProductVariationStatus.OutOfStock
                            : variationDto.Status,
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
                            Stock = v.Stock,
                            Status = v.Status
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

        public async Task<CreateProductDto> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken)
        {
            if (productId == Guid.Empty)
            {
                throw new Exception("Đã có lỗi khi lấy dữ liệu");
            }

            var product = await _productRepo.GetProductByIdAsync(productId, cancellationToken);

            var userIds = new List<Guid>();

            if (product?.CreatedBy != null && product.CreatedBy != Guid.Empty)
                userIds.Add(product.CreatedBy);

            if (product?.UpdatedBy != null && product.UpdatedBy != Guid.Empty)
                userIds.Add(product.UpdatedBy);

            var users = await _userRepository.GetUsersByIdsAsync(userIds, cancellationToken);

            var creater = users.FirstOrDefault(u => u.UserId == product?.CreatedBy);
            var updater = users.FirstOrDefault(u => u.UserId == product?.UpdatedBy);


            var productDto = new CreateProductDto
            {
                ProductCode = product?.ProductCode,
                PrivewImageUrl = product?.PrivewImage,
                ProductName = product?.ProductName,
                CategoryId = product?.CategoryId,
                CategoryName = product?.Category?.CategoryName,
                Description = product?.Description,
                Price = product.Price,
                Stock = product.Stock,
                Origin = product.Origin,
                Brand = product.Brand?.BrandName,
                Note = product.Note,
                ProductStatus = product.ProductStatus,

                ProductImageUrls = product.ProductImages?.Select(pi => pi.ImageUrl).ToList() ?? new List<string>(),

                ProductAttributes = product.ProductAttributes?.Select(attr => new ProductAttributeDto
                {
                    AttributeId = attr?.AttributeId,
                    Value = attr?.Value
                }).ToList() ?? new List<ProductAttributeDto>(),

                ProductVariations = product.ProductVariations?.Select(v => new ProductVariationDto
                {
                    ProductVariationId = v.ProductVariationId,
                    TypeName = v.TypeName,
                    Price = v.Price,
                    Stock = v.Stock,
                    Status = v.Status,
                }).ToList() ?? new List<ProductVariationDto>(),

                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                CreatorName = creater?.FullName,
                UpdaterName = updater?.FullName
            };
            return productDto;
        }

        public async Task<bool> UpdateProductAsync(Guid productId, ClaimsPrincipal user, CreateProductDto dto, CancellationToken cancellationToken)
        {
            var product = await _productRepo.GetProductByIdAsync(productId, cancellationToken);
            var userId = GetUserIdFromClaims.GetUserId(user);

            if (product == null)
            {
                throw new Exception("Sản phẩm không tồn tại");
            }

            try
            {
                product.ProductName = dto.ProductName;
                product.PrivewImage = dto.PrivewImageUrl;
                product.Price = dto.Price;
                product.Stock = dto.Stock;
                product.Origin = dto.Origin;
                product.HasVariations = dto.HasVariations;
                product.CategoryId = dto.CategoryId;
                product.BrandId = dto.BrandId;
                product.ProductStatus = dto.ProductStatus;
                product.Note = dto.Note;
                product.UpdatedAt = DateTime.UtcNow;
                product.UpdatedBy = userId;
                product.Description = dto.Description;

                await _productRepo.UpdateProduct(product, cancellationToken);

                await _producImage.DeleteByProductIdAsync(productId, cancellationToken);
                if (dto.ProductImageUrls?.Any() == true)
                {
                    await _producImage.AddRangeAsync(productId, dto.ProductImageUrls, cancellationToken);
                }

                await _productAttribute.DeleteByProductIdAsync(productId, cancellationToken);

                if (dto.ProductAttributes?.Any() == true)
                {
                    var productAttributes = dto.ProductAttributes.Select(attrDto => new ProductAttribute
                    {
                        ProductId = productId,
                        AttributeId = attrDto.AttributeId,
                        Value = attrDto.Value,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userId
                    }).ToList();

                    await _productAttribute.AddRangeAsync(productAttributes, cancellationToken);
                }

                //await _productVariation.DeleteByProductIdAsync(productId, cancellationToken);
                if (dto.HasVariations && dto.ProductVariations?.Any() == true)
                {
                    var existingVariations = await _productVariation.GetAllByProductIdAsync(productId, cancellationToken);
                    var existingDict = existingVariations.ToDictionary(v => v.ProductVariationId);

                    var updateList = new List<ProductVariations>();
                    var addList = new List<ProductVariations>();

                    // Tập hợp ID biến thể mà client gửi lên
                    var incomingIds = dto.ProductVariations
                                        .Where(v => v.ProductVariationId.HasValue)
                                        .Select(v => v.ProductVariationId!.Value)
                                        .ToHashSet();

                    foreach (var variationDto in dto.ProductVariations)
                    {
                        if (variationDto.ProductVariationId != null &&
                            existingDict.TryGetValue(variationDto.ProductVariationId.Value, out var existing))
                        {
                            // Update
                            existing.TypeName = variationDto.TypeName;
                            existing.Price = variationDto.Price;
                            existing.Stock = variationDto.Stock;
                            existing.Status = variationDto.Stock == 0
                            ? ProductVariationStatus.OutOfStock
                            : variationDto.Status;
                            existing.UpdatedAt = DateTime.UtcNow;
                            existing.UpdatedBy = userId;

                            updateList.Add(existing);
                        }
                        else
                        {
                            // Add
                            var newVariation = new ProductVariations
                            {
                                ProductVariationId = Guid.NewGuid(),
                                ProductId = productId,
                                TypeName = variationDto.TypeName,
                                Price = variationDto.Price,
                                Status = variationDto.Stock == 0
                                    ? ProductVariationStatus.OutOfStock
                                    : variationDto.Status,
                                Stock = variationDto.Stock,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = userId
                            };

                            addList.Add(newVariation);
                        }
                    }

                    // Xác định các biến thể cần xóa
                    //var toDelete = existingVariations
                    //    .Where(v => !incomingIds.Contains(v.ProductVariationId))
                    //    .ToList();

                    //if (toDelete.Any())
                    //    await _productVariation.DeleteRangeAsync(toDelete, cancellationToken);

                    if (addList.Any())
                        await _productVariation.AddRangeAsync(addList, cancellationToken);

                    if (updateList.Any())
                        await _productVariation.UpdateRangeAsync(updateList, cancellationToken);
                }


                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Cập nhật sản phẩm thất bại", ex);
            }
        }

        public async Task<int> SoftDeleteProductsAsync( IEnumerable<Guid> productIds, ClaimsPrincipal user, CancellationToken ct)
        {
            var ids = productIds?.Where(x => x != Guid.Empty).Distinct().ToList() ?? new();
            if (ids.Count == 0) return 0;

            var userId = GetUserIdFromClaims.GetUserId(user);
            Guid updatedBy = userId == Guid.Empty ? Guid.Empty : userId;

            var utcNow = DateTime.UtcNow;
            return await _productRepo.SoftDeleteByIdsAsync(ids, updatedBy, utcNow, ct);
        }


        /// <summary> 
        /// Store 
        /// </summary>>
        /// 

        public async Task<PagedResult<StoreProducts>> StoreGetAllProductAsync(ProductFilterRequest filters, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var categoryTree = await _category.GetAllCategoriesFlatAsync(cancellationToken);
            List<int>? categoryIds = null;

            if (filters.CategoryId.HasValue)
            {
                // categoryTree là danh mục toàn hệ thống đã lấy từ DB hoặc cache
                categoryIds = GetAllSubCategoryIds(categoryTree, filters.CategoryId.Value);
            }
            var domainFilter = new ProductFilterDto
            {
                ProductCode = filters.ProductCode,
                Keyword = filters.Keyword,
                SortBy = filters.SortBy,
                //CategoryId = filters.CategoryId,
                BrandId = filters.BrandId,
                Status = ProductStatus.Active,
                CategoryIds = categoryIds
            };

            var query = _productRepo.GetFilteredProducts(domainFilter, cancellationToken);

            var totalItems = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var productsPage = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
            

            var items = productsPage.Select(p =>
            {

                return new StoreProducts
                {
                    ProductId = p.ProductId,
                    ProductCode = p.ProductCode,
                    PrivewImageUrl = p.PrivewImage,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    HasVariations = p.HasVariations,

                    ProductImageUrls = p.ProductImages != null
                        ? p.ProductImages.Select(pi => pi.ImageUrl).ToList()
                        : new List<string>(),

                    ProductVariations = p.ProductVariations != null
                        ? p.ProductVariations.Select(v => new ProductVariationDto
                        {
                            ProductVariationId = v.ProductVariationId,
                            TypeName = v.TypeName,
                            Price = v.Price,
                        }).ToList()
                        : new List<ProductVariationDto>(),
                };
            }).ToList();

            return new PagedResult<StoreProducts>
            {
                Items = items,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize,
            };
        }

        public List<int> GetAllSubCategoryIds(List<Category> allCategories, int parentId)
        {
            var result = new List<int> { parentId };

            var childCategories = allCategories.Where(c => c.ParentId == parentId).ToList();

            foreach (var child in childCategories)
            {
                result.AddRange(GetAllSubCategoryIds(allCategories, child.CategoryId));
            }

            return result;
        }

        public async Task<StoreProducts> StoreGetProductByCodeAsync(string productCode, CancellationToken cancellationToken)
        {
            if (productCode == null)
            {
                throw new Exception("Đã có lỗi khi lấy dữ liệu");
            }

            var product = await _productRepo.GetProductByCodeAsync(productCode, cancellationToken);

            var productDto = new StoreProducts
            {
                ProductId = product.ProductId,
                ProductCode = product?.ProductCode,
                PrivewImageUrl = product?.PrivewImage,
                ProductName = product?.ProductName,
                CategoryId = product?.CategoryId,
                CategoryName = product?.Category?.CategoryName,
                Description = product?.Description,
                Price = product.Price,
                ProductStatus = product.ProductStatus,

                ProductImageUrls = product.ProductImages?.Select(pi => pi.ImageUrl).ToList() ?? new List<string>(),

                ProductAttributes = product.ProductAttributes?
                    .Where(attr => attr != null && attr.Attribute != null)
                    .Select(attr => new ProductAttributeDto
                    {
                        AttributeId = attr.AttributeId ?? 0,
                        AttributeName = attr.Attribute.Name,
                        Value = attr.Value
                    })
                    .ToList() ?? new List<ProductAttributeDto>(),

                ProductVariations = product.ProductVariations?.Select(v => new ProductVariationDto
                {
                    ProductVariationId = v.ProductVariationId,
                    TypeName = v.TypeName,
                    Price = v.Price,
                    Stock = v.Stock
                }).ToList() ?? new List<ProductVariationDto>(),

            };
            return productDto;
        }


        public async Task<int> SoftDeleteByIdVarition(IEnumerable<Guid> ids, Guid updatedBy, CancellationToken ct)
        {
            if (ids == null) return 0;
            var utcNow = DateTime.UtcNow;

            return await _productVariation.SoftDeleteByIdsAsync(ids, updatedBy, utcNow, ct);
        }
    }
}
