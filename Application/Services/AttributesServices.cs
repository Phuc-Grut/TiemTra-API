using Application.DTOs.Attributes;
using Application.Interface;
using AutoMapper;
using Domain.Data.Entities;
using Infrastructure.Interface;
using System.Security.Claims;

namespace Application.Services
{
    public class AttributesServices : IAttributesServices
    {
        private readonly IAttributesRepository _attributesRepository;
        private readonly IMapper _mapper;

        public AttributesServices(IAttributesRepository attributesRepository, IMapper mapper)
        {
            _attributesRepository = attributesRepository;
            _mapper = mapper;
        }
        public async Task<AttributesDTO> AddAttribute(AttributesDTO attributesDTO, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            var newAttribute = new Attributes
            {
                Name = attributesDTO.Name,
                Description = attributesDTO.Description,
            };

            var result = await _attributesRepository.AddAttribute(newAttribute, cancellationToken);

            return _mapper.Map<AttributesDTO>(result);
        }

        public Task<IEnumerable<AttributesDTO>> GetAllAttributes(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
