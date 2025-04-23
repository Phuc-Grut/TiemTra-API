using Application.DTOs;
using Application.DTOs.Attributes;
using Application.DTOs.Category;
using Application.DTOs.User;
using Application.Interface;
using AutoMapper;
using Domain.Data.Entities;
using Domain.DTOs.Category;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using System.Security.Claims;

namespace Application.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private static readonly Random _random = new Random();
        private readonly IUserRepository _userRepository;
        private readonly ICategoryAttributesRepository _categoryAttRepo;
        private readonly IAttributesRepository _attributesRepository;
        private readonly IProductRepository _productRepository;

        public CategoryServices(ICategoryRepository categoryRepository, IMapper mapper, IUserRepository userRepository, 
            IAttributesRepository attributesRepository, ICategoryAttributesRepository categoryAttributesRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _attributesRepository = attributesRepository;
            _categoryAttRepo = categoryAttributesRepository;
            _productRepository = productRepository;
        }

        public async Task<CategoryDto> AddCategory(UpCategoryDto categoryDto, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            try
            {
                var userId = GetUserIdFromClaims.GetUserId(user);

                int? parentId = (categoryDto.ParentId.HasValue && categoryDto.ParentId > 0) ? categoryDto.ParentId : null;

                var newCategory = new Category
                {
                    CategoryId = await GenerateUniqueCategoryId(cancellationToken),
                    CategoryName = categoryDto.CategoryName,
                    Description = categoryDto.Description,
                    ParentId = parentId,
                    CreatedBy = userId,
                    UpdatedBy = userId
                };

                var result = await _categoryRepository.AddCategory(newCategory, cancellationToken);
                return _mapper.Map<CategoryDto>(newCategory);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"bug {ex.InnerException?.Message}");
                return null;
            }
        }

        public async Task<object> GetCategoryById(int categoryId, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = _categoryRepository.GetCategoriesQuery();
            var breadcrumbs = await GetCategoryBreadcrumbs(categoryId, cancellationToken);

            var categoryExists = await _categoryRepository.CategoryExists(categoryId);
            if (!categoryExists)
                throw new Exception("Danh mục không tồn tại.");

            var currentCategory = await _categoryRepository.GetCategoryById(categoryId, cancellationToken);

            var subCategories = await _categoryRepository.GetSubCategories(categoryId, cancellationToken);

            if (subCategories != null && subCategories.Any())
            {
                var pageSubCategories = subCategories
                    .OrderBy(c => c.CategoryName)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var totalItems = subCategories.Count;

                var userIds = pageSubCategories
                    .Select(c => c.CreatedBy)
                    .Union(pageSubCategories.Select(c => c.UpdatedBy))
                    .Distinct()
                    .ToList();

                var users = await _userRepository.GetUsersByIdsAsync(userIds, cancellationToken);

                var categoryDtos = pageSubCategories.Select(c =>
                {
                    var creator = users.FirstOrDefault(u => u.UserId == c.CreatedBy);
                    var updater = users.FirstOrDefault(u => u.UserId == c.UpdatedBy);
                    return new CategoryDto
                    {
                        CategoryId = c.CategoryId,
                        CategoryName = c.CategoryName,
                        Description = c.Description,
                        ParentId = c.ParentId,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt,
                        CreatorName = creator?.FullName,
                        UpdaterName = updater?.FullName,
                    };
                }).ToList();

                return new
                {
                    Type = "Categories",
                    CurrentCategory = new
                    {
                        CategoryId = currentCategory.CategoryId,
                        CategoryName = currentCategory.CategoryName,
                        Breadcrumbs = breadcrumbs.ToArray()
                    },
                    Data = new PagedResult<CategoryDto>
                    {
                        Items = categoryDtos,
                        TotalItems = totalItems,
                        TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                        CurrentPage = pageNumber,
                        PageSize = pageSize
                    }
                };
            }

            var attributes = await _categoryAttRepo.GetAttributesByCategory(categoryId, cancellationToken);

            if (attributes?.Any() == true)
            {
                var userIds = attributes
                    .Select(a => a.CreatedBy)
                    .Union(attributes.Select(a => a.UpdatedBy))
                    .Distinct()
                    .ToList();

                var users = await _userRepository.GetUsersByIdsAsync(userIds, cancellationToken);
                var user = await _userRepository.GetUsersByIdsAsync(userIds, cancellationToken);

                var totalItems = attributes.Count;

                var attributesDtoList = attributes.Select(atb => 
                {
                    var creator = users.FirstOrDefault(u => u.UserId == atb.CreatedBy);
                    var updater = users.FirstOrDefault(u => u.UserId == atb.UpdatedBy);

                    return new AttributesDTO
                    {
                        AttributeId = atb.AttributeId,
                        Name = atb.Name,
                        Description = atb.Description,
                        CreatorName = creator?.FullName,
                        UpdaterName = updater?.FullName,
                    };
                }).ToList();

                return new
                {
                    Type = "Attributes",
                    CurrentCategory = new
                    {
                        CategoryId = currentCategory.CategoryId,
                        CategoryName = currentCategory.CategoryName,
                        Breadcrumbs = breadcrumbs.ToArray()
                    },
                    Data = new PagedResult<AttributesDTO>
                    {
                        Items = attributesDtoList,
                        TotalItems = attributes.Count,
                        TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                        CurrentPage = pageNumber,
                        PageSize = pageSize
                    }
                };
            }

            return new
            {
                CurrentCategory = new
                {
                    CategoryId = currentCategory.CategoryId,
                    CategoryName = currentCategory.CategoryName,
                    Breadcrumbs = breadcrumbs.ToArray()
                },
                Type = "Empty",
                Data = new List<object>()
            };
        }

        private async Task<List<CategoryBreadcrumbDto>> GetCategoryBreadcrumbs(int categoryId, CancellationToken cancellationToken)
        {
            var breadcrumbs = new List<CategoryBreadcrumbDto>();
            var current = await _categoryRepository.GetCategoryById(categoryId, cancellationToken);

            while (current != null)
            {
                breadcrumbs.Insert(0, new CategoryBreadcrumbDto
                {
                    CategoryId = current.CategoryId,
                    CategoryName = current.CategoryName
                });

                if (current.ParentId == null) break;

                current = await _categoryRepository.GetCategoryById(current.ParentId.Value, cancellationToken);
            }

            return breadcrumbs;
        }

        public async Task<(bool CanDelete, string Message)> CheckIfCategoryCanBeDeleted(int categoryId, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetCategoryById(categoryId, cancellationToken);

            if (category == null)
                return (false, "Danh mục không tồn tại.");

             //check con
            var hasChildren = await _categoryRepository.HasChildCategories(categoryId, cancellationToken);
            if (hasChildren)
                return (false, $"Danh mục \"{category.CategoryName}\" vẫn còn danh mục con. Vui lòng xoá danh mục con trước.");

            var linkedProductCount = await _productRepository.CountProductByCategory(categoryId, cancellationToken);
            if (linkedProductCount > 0)
                return (true, $"Danh mục \"{category.CategoryName}\" đang liên kết với {linkedProductCount} sản phẩm. Bạn có xác nhận xóa");

            if (linkedProductCount == 0)
            {
                var attributeCount = await _categoryAttRepo.CountAttributesByCategory(categoryId, cancellationToken);
                if (attributeCount > 0)
                    return (true, $"Danh mục \"{category.CategoryName}\" đang liên kết với {attributeCount} thuộc tính. Bạn có xác nhận xóa");
            }

            return (true, $"Bạn có chắc chắn muốn xoá danh mục \"{category.CategoryName}\"?");
        }

        public async Task<List<CategoryCheckResult>> CheckCanDeleteCategories(List<int> categoryIds, CancellationToken cancellationToken)
        {
            var results = new List<CategoryCheckResult>();

            foreach (var id in categoryIds)
            {
                var category = await _categoryRepository.GetCategoryById(id, cancellationToken);

                if (category == null)
                {
                    results.Add(new CategoryCheckResult
                    {
                        CategoryId = id,
                        CategoryName = $"[ID {id}]",
                        CanDelete = false,
                        Message = "Danh mục không tồn tại."
                    });
                    continue;
                }

                var (canDelete, message) = await CheckIfCategoryCanBeDeleted(id, cancellationToken);
                results.Add(new CategoryCheckResult
                {
                    CategoryId = id,
                    CategoryName = category.CategoryName,
                    CanDelete = canDelete,
                    Message = message
                });
            }

            return results;
        }

        public async Task<List<CategoryDeleteResult>> DeleteCategoriesAsync(List<int> categoryIds, CancellationToken cancellationToken)
        {
            var results = new List<CategoryDeleteResult>();

            foreach (var id in categoryIds)
            {
                var category = await _categoryRepository.GetCategoryById(id, cancellationToken);
                if (category == null)
                {
                    results.Add(new CategoryDeleteResult
                    {
                        CategoryId = id,
                        CategoryName = $"[ID {id}]",
                        IsDeleted = false,
                        Message = "Danh mục không tồn tại."
                    });
                    continue;
                }

                var check = await CheckIfCategoryCanBeDeleted(id, cancellationToken);
                if (!check.CanDelete)
                {
                    results.Add(new CategoryDeleteResult
                    {
                        CategoryId = id,
                        CategoryName = category.CategoryName,
                        IsDeleted = false,
                        Message = check.Message
                    });
                    continue;
                }

                // Thực hiện xoá
                await _productRepository.RemoveCategoryFromProducts(id, cancellationToken);
                await _categoryAttRepo.RemoveAllCategoreyFromAttributes(id, cancellationToken);
                var deleted = await _categoryRepository.DeleteCategory(id, cancellationToken);

                results.Add(new CategoryDeleteResult
                {
                    CategoryId = id,
                    CategoryName = category.CategoryName,
                    IsDeleted = deleted,
                    Message = deleted ? "Xoá thành công." : "Xoá thất bại."
                });
            }

            return results;
        }

        public async Task<bool> UpdateCategory(int categoryId, UpCategoryDto categoryDto, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetCategoryById(categoryId, cancellationToken);
            var userId = GetUserIdFromClaims.GetUserId(user);

            if (category == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(categoryDto.CategoryName))
            {
                category.CategoryName = categoryDto.CategoryName;
            }

            if (!string.IsNullOrEmpty(categoryDto.Description))
            {
                category.Description = categoryDto.Description;
            }

            //if (categoryDto.ParentId.HasValue)
            //{
            //    if (categoryDto.ParentId == category.CategoryId)
            //        return false;

            //    var parentCategory = await _categoryRepository.GetCategoryById(categoryDto.ParentId.Value, cancellationToken);
            //    if (parentCategory == null)
            //    {
            //        return false;
            //    }

            //    category.ParentId = categoryDto.ParentId;
            //}

            category.UpdatedBy = userId;
            category.UpdatedAt = DateTime.UtcNow;

            await _categoryRepository.UpdateCategory(category, cancellationToken);
            return true;
        }

        public async Task<PagedResult<CategoryDto>> GetAllCategories(CategoryFilterDto filters, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = _categoryRepository.GetCategoriesQuery();

            var keyword = filters.Keyword?.Trim();

            if (!string.IsNullOrEmpty(filters.Keyword))
            {
                query = query.Where(c => EF.Functions.Like(c.CategoryName, $"%{keyword}"));
            }

            int totalItems = await query.CountAsync(cancellationToken);

            var categories = await query
                .OrderBy(c => c.CategoryName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            // Lấy danh sách ID của người tạo & cập nhật
            var userIds = categories.Select(c => c.CreatedBy).Union(categories.Select(c => c.UpdatedBy)).Distinct().ToList();

            var users = await _userRepository.GetUsersByIdsAsync(userIds, cancellationToken);

            var categoryDtos = categories.Select(c =>
            {
                var creator = users.FirstOrDefault(u => u.UserId == c.CreatedBy);
                var updater = users.FirstOrDefault(u => u.UserId == c.UpdatedBy);
                return new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    Description = c.Description,
                    ParentId = c.ParentId,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    CreatorName = creator?.FullName,
                    UpdaterName = updater?.FullName,
                };
            }).ToList();

            return new PagedResult<CategoryDto>
            {
                Items = categoryDtos,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        //tạo id
        private async Task<int> GenerateUniqueCategoryId(CancellationToken cancellationToken)
        {
            var existingIds = (await _categoryRepository.GetAllCategories(cancellationToken))
                                .Select(c => c.CategoryId)
                                .ToHashSet();

            int newId;
            do
            {
                newId = new Random().Next(100, 999);
            }
            while (existingIds.Contains(newId));
            return newId;
        }

        //public async Task<IEnumerable<Category>> GetAllCategories(CancellationToken cancellationToken)
        //{
        //    return await _categoryRepository.GetAllCategories(cancellationToken);
        //}

        //public async Task<IEnumerable<CategoryDto>> FilterCategories(CategoryFilterDto filters, CancellationToken cancellationToken)
        //{
        //    var categories = await _categoryRepository.FilterCategories(filters, cancellationToken);

        //    return categories.OrderBy(c => c.CategoryName)
        //        .Select(c => new CategoryDto
        //        {
        //            CategoryId = c.CategoryId,
        //            CategoryName = c.CategoryName,
        //            ParentId = c.ParentId
        //        })
        //        .ToList();
        //}
    }
}