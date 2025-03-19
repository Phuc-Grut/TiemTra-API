using Application.DTOs.Category;
using Application.Interface;
using Domain.Data.Entities;
using Infrastructure.Interface;
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

        public async Task<Category> AddCategory(CategoryDto categoryDto, ClaimsPrincipal user, CancellationToken cancellationToken)
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
                Console.WriteLine($"Loi khi them danh muc: {ex.Message}");
                Console.WriteLine($"🔎 Chi tiết lỗi: {ex.InnerException?.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategories(CancellationToken cancellationToken)
        {
            return await _categoryRepository.GetAllCategories(cancellationToken);
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
    }
}
