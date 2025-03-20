using Application.DTOs.Attributes;
using Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IAttributesServices
    {
        Task<AttributesDTO> AddAttribute(AttributesDTO attributesDTO, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<IEnumerable<AttributesDTO>> GetAllAttributes(CancellationToken cancellationToken);
    }
}
