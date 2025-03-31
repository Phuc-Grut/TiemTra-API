using Application.DTOs.Attributes;
using System.Security.Claims;

namespace Application.Interface
{
    public interface IAttributesServices
    {
        Task<AttributesDTO> AddAttribute(AddAttributesDTO attributesDTO, ClaimsPrincipal user, CancellationToken cancellationToken);

        Task<IEnumerable<AttributesDTO>> GetAllAttributes(CancellationToken cancellationToken);
    }
}