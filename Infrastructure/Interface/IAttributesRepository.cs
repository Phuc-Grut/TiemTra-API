using Domain.Data.Entities;

namespace Infrastructure.Interface
{
    public interface IAttributesRepository
    {
        IQueryable<Attributes> GetAttributesQuery();
        Task<IEnumerable<Attributes>> GetAllAttributes(CancellationToken cancellationToken);
        Task<Attributes> AddAttribute(Attributes attributes, CancellationToken cancellationToken);

        Task<bool> AttributeExists(int attributeId);
    }
}