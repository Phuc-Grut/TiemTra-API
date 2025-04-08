using Domain.Data.Entities;

namespace Infrastructure.Interface
{
    public interface ICategoryAttributesRepository
    {
        Task AddAttributesToCategory(CategoryAttributes categoryAttribute, CancellationToken cancellationToken);

        Task RemoveAllAttributesFromCategory(int categoryId, CancellationToken cancellationToken);

        Task<List<Attributes>> GetAttributesByCategory(int categoryId, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(int categoryId, int attributeId);
        Task<int> CountAttributesByCategory(int categoryId, CancellationToken cancellationToken);
    }
}