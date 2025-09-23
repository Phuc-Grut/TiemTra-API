using Application.DTOs;
using Application.DTOs.Admin.Attributes;
using Application.Interface;
using AutoMapper;
using Domain.Data.Entities;
using Domain.Interface;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using System.Security.Claims;

namespace Application.Services
{
    public class AttributesServices : IAttributesServices
    {
        private readonly IAttributesRepository _attributesRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryAttributesRepository _categoryAttributesRepository;

        public AttributesServices(IAttributesRepository attributesRepository, IMapper mapper, IUserRepository userRepository, ICategoryAttributesRepository categoryAttributesRepository)
        {
            _attributesRepository = attributesRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _categoryAttributesRepository = categoryAttributesRepository;
        }

        public async Task<AttributesDTO> AddAttribute(AddAttributesDTO attributesDTO, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            var userId = GetUserIdFromClaims.GetUserId(user);
            var newAttribute = new Attributes
            {
                AttributeId = await GenerateUniqueAttributesId(cancellationToken),
                Name = attributesDTO.AttributeName,
                Description = attributesDTO.Description,
                CreatedBy = userId,
                UpdatedBy = userId
            };

            var result = await _attributesRepository.AddAttribute(newAttribute, cancellationToken);

            return _mapper.Map<AttributesDTO>(result);
        }

        private async Task<int> GenerateUniqueAttributesId(CancellationToken cancellationToken)
        {
            var existingIds = (await _attributesRepository.GetAllAttributes(cancellationToken))
                                .Select(c => c.AttributeId)
                                .ToHashSet();

            int newId;
            do
            {
                newId = new Random().Next(100, 999);
            }
            while (existingIds.Contains(newId));

            return newId;
        }

        public async Task<PagedResult<AttributesDTO>> GetAllAttributes(AttributesFilterDTO filters, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = _attributesRepository.GetAttributesQuery();

            var keyword = filters.Keyword?.Trim();

            if (!string.IsNullOrEmpty(filters.Keyword))
            {
                query = query.Where(atb => EF.Functions.Like(atb.Name, $"%{keyword}%"));
            }

            int totalItems = await query.CountAsync(cancellationToken);

            var attributes = await query.OrderBy(atb => atb.Name)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync(cancellationToken);

            var userIds = attributes.Select(atb => atb.CreatedBy).Union(attributes.Select(atb => atb.UpdatedBy)).Distinct().ToList();

            var users = await _userRepository.GetUsersByIdsAsync(userIds, cancellationToken);

            var attributesDto = attributes.Select(atb =>
            {
                var creator = users.FirstOrDefault(u => u.UserId == atb.CreatedBy);
                var updater = users.FirstOrDefault(u => u.UserId == atb.UpdatedBy);

                return new AttributesDTO
                {
                    AttributeId = atb.AttributeId,
                    Name = atb.Name,
                    Description = atb.Description,
                    UpdatedAt = atb.UpdatedAt,
                    CreatedAt = atb.CreatedAt,
                    CreatorName = creator?.FullName,
                    UpdaterName = updater?.FullName,
                };
            }).ToList();

            return new PagedResult<AttributesDTO>
            {
                Items = attributesDto,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<bool> DeleteAttribute(List<int> attributeIds, CancellationToken cancellationToken)
        {
            if (attributeIds == null || attributeIds.Count == 0)
            {
                return false;
            }

            foreach (var attributeId in attributeIds)
            {
                await _categoryAttributesRepository.RemoveAttributeFromAllCategories(attributeId, cancellationToken);
            }

            await _attributesRepository.DeleteAttribute(attributeIds, cancellationToken);
            return true;
        }

        public async Task<bool> UpdateAttribute(int attributeId, AddAttributesDTO attributesDTO, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            var userId = GetUserIdFromClaims.GetUserId(user);

            var attribute = await _attributesRepository.GetAttributeById(attributeId, cancellationToken);
            if (attribute == null)
            {
                return false;
            }
            if (!string.IsNullOrEmpty(attributesDTO.AttributeName))
            {
                attribute.Name = attributesDTO.AttributeName;
            }
            if (!string.IsNullOrEmpty(attributesDTO.Description))
            {
                attribute.Description = attributesDTO.Description;
            }

            attribute.UpdatedBy = userId;
            attribute.UpdatedAt = DateTime.Now;

            await _attributesRepository.UpdateAttribute(attribute, cancellationToken);
            return true;
        }
    }
}