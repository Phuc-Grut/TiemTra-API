using Application.DTOs;
using Application.DTOs.Category;
using Application.Interface;
using Domain.Data.Entities;
using Domain.DTOs.Category;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using System.Security.Claims;

namespace Application.Services
{
    public class CategoryServices : ICategoryServices
    {
        private ICategoryRepository _categoryRepository;
        private static readonly Random _random = new Random();

        public CategoryServices(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> AddCategory(UpCategoryDto categoryDto, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            try
            {
                var userId = GetUserIdFromClaims.GetUserId(user);

                int? parentId = (categoryDto.ParentId.HasValue && categoryDto.ParentId > 0) ? categoryDto.ParentId : null;

                var newCategory = new Category
                {
                    CategoryId = await GenerateUniqueCategoryId(cancellationToken),
                    CategoryName = categoryDto.CategoryName,
                    ParentId = parentId,
                    CreatedBy = userId,
                    UpdatedBy = userId
                };

                var result = await _categoryRepository.AddCategory(newCategory, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"bug {ex.InnerException?.Message}");
                return null;
            }
        }

        public async Task<Category> GetCategoryById(int categoryId, CancellationToken cancellationToken)
        {
            return await _categoryRepository.GetCategoryById(categoryId, cancellationToken);
        }

        //Tạo id
        public async Task<int> GenerateUniqueCategoryId(CancellationToken cancellationToken)
        {
            // Lấy danh sách ID hiện có từ danh sách danh mục
            var existingIds = (await _categoryRepository.GetAllCategories(cancellationToken))
                                .Select(c => c.CategoryId)
                                .ToHashSet();

            int newId;
            do
            {
                newId = new Random().Next(100, 999);
            }
            while (existingIds.Contains(newId)); // check ID trùng

            return newId;
        }
        

        public async Task<bool> DeleteCategory(int categoryId, CancellationToken cancellationToken)
        {
            return await _categoryRepository.DeleteCategory(categoryId, cancellationToken);
        }

        public async Task<bool> UpdateCategory(int categoryId, UpCategoryDto categoryDto, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetCategoryById(categoryId, cancellationToken);

            if (category == null) 
            {
                return false;
            }

            if (!string.IsNullOrEmpty(categoryDto.CategoryName))
            {
                category.CategoryName = categoryDto.CategoryName;
            }

            if (categoryDto.ParentId.HasValue)
            {
                var parentCategory = await _categoryRepository.GetCategoryById(categoryDto.ParentId.Value, cancellationToken);
                if (parentCategory == null)
                {
                    return false;
                }
                category.ParentId = categoryDto.ParentId;
            }
            await _categoryRepository.UpdateCategory(category, cancellationToken);
            return true;
        }

        public async Task<PagedResult<CategoryDto>> GetAllCategories(CategoryFilterDto filters, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = _categoryRepository.GetCategoriesQuery();

            if (!string.IsNullOrEmpty(filters.Keyword))
            {
                query = query.Where(c => c.CategoryName.ToLower().Contains(filters.Keyword.ToLower()));
            }

            //if (filters.ParentId.HasValue)
            //{
            //    query = query.Where(c => c.ParentId == filters.ParentId);
            //}

            //if (filters.CategoryId.HasValue)
            //{
            //    query = query.Where(c => c.CategoryId == filters.CategoryId);
            //}

            int totalItems = await query.CountAsync(cancellationToken);

            var categories = await query
                .OrderBy(c => c.CategoryName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var categoryDtos = categories.Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                ParentId = c.ParentId
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
