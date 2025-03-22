using Domain.Data.Entities;

namespace Infrastructure.Interface
{
    public interface IAttributesRepository
    {
        Task<Attributes> AddAttribute(Attributes attributes, CancellationToken cancellationToken);

        Task<IEnumerable<Attributes>> GetAllAttributes(CancellationToken cancellationToken);

        Task<bool> AttributeExists(int attributeId);
    }
}