using Domain.Data.Entities;

namespace Domain.Interface
{
    public interface IProductAttributeRepository
    {
        Task AddRangeAsync(List<ProductAttribute> attributes, CancellationToken cancellationToken);

        Task DeleteByProductIdAsync(Guid productId, CancellationToken cancellationToken);
    }
}