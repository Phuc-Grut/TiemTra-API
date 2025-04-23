using Domain.Data.Entities;

namespace Infrastructure.Interface
{
    public interface ICategoryAttributesRepository
    {
        Task<List<int>> GetAttributeIdsByCategory(int categoryId, CancellationToken cancellationToken);

        Task RemoveAllCategoreyFromAttributes(int categoryId, CancellationToken cancellationToken);
        Task RemoveAsync(int categoryId, List<int> attributeIds, CancellationToken cancellationToken);
        Task AddAsync(CategoryAttributes categoryAttribute, CancellationToken cancellationToken);
        Task<List<Attributes>> GetAttributesByCategory(int categoryId, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(int categoryId, int attributeId);
        Task<int> CountAttributesByCategory(int categoryId, CancellationToken cancellationToken);

        Task RemoveAttributeFromAllCategories(int attributeId, CancellationToken cancellationToken);

    }
}