using Domain.Data.Entities;

namespace Domain.Interface
{
    public interface IAttributesRepository
    {
        IQueryable<Attributes> GetAttributesQuery();

        Task<IEnumerable<Attributes>> GetAllAttributes(CancellationToken cancellationToken);

        Task<Attributes> AddAttribute(Attributes attributes, CancellationToken cancellationToken);

        Task<bool> AttributeExists(int attributeId);

        Task DeleteAttribute(List<int> attributeIds, CancellationToken cancellationToken);

        Task<Attributes> GetAttributeById(int attributeId, CancellationToken cancellationToken);

        Task<bool> UpdateAttribute(Attributes attributes, CancellationToken cancellationToken);
    }
}