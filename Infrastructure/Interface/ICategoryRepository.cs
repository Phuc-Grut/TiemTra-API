using Domain.Data.Entities;

//using Domain.DTOs.Category;

namespace Infrastructure.Interface
{
    public interface ICategoryRepository
    {
        IQueryable<Category> GetCategoriesQuery();

        Task<Category> AddCategory(Category category, CancellationToken cancellationToken);

        Task<bool> DeleteCategory(int categoryId, CancellationToken cancellationToken);

        Task<Category> GetCategoryById(int categoryId, CancellationToken cancellationToken);

        Task<IEnumerable<Category>> GetAllCategories(CancellationToken cancellationToken);

        Task<bool> UpdateCategory(Category category, CancellationToken cancellationToken);

        Task<bool> HasChildCategories(int categoryId);

        Task<bool> CategoryExists(int categoryId);

        Task<List<Category>> GetSubCategories(int parentId, CancellationToken cancellationToken);

        //Task<IEnumerable<Category>> FilterCategories(CategoryFilterDto filters, CancellationToken cancellationToken);
    }
}