using Application.DTOs.Attributes;
using Application.Interface;
using AutoMapper;
using Domain.Data.Entities;
using Infrastructure.Interface;
using Infrastructure.Repositories;
using Shared.Common;
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
            var userId = GetUserIdFromClaims.GetUserId(user);
            var newAttribute = new Attributes
            {
                AttributeId = await GenerateUniqueAttributesId(cancellationToken),
                Name = attributesDTO.Name,
                Description = attributesDTO.Description,
                CreatedBy = userId,
                UpdatedBy = userId
            };

            var result = await _attributesRepository.AddAttribute(newAttribute, cancellationToken);

            return _mapper.Map<AttributesDTO>(result);
        }

        public Task<IEnumerable<AttributesDTO>> GetAllAttributes(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<int> GenerateUniqueAttributesId(CancellationToken cancellationToken)
        {
            // Lấy danh sách ID hiện có từ danh sách danh mục
            var existingIds = (await _attributesRepository.GetAllAttributes(cancellationToken))
                                .Select(c => c.AttributeId)
                                .ToHashSet();

            int newId;
            do
            {
                newId = new Random().Next(100, 999);
            }
            while (existingIds.Contains(newId)); // check ID trùng

            return newId;
        }

    }
}
