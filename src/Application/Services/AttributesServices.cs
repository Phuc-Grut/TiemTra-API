using Application.DTOs;
using Application.DTOs.Attributes;
using Application.DTOs.User;
using Application.Interface;
using AutoMapper;
using Domain.Data.Entities;
using Infrastructure.Interface;
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

        public AttributesServices(IAttributesRepository attributesRepository, IMapper mapper, IUserRepository userRepository)
        {
            _attributesRepository = attributesRepository;
            _mapper = mapper;
            _userRepository = userRepository;
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
    }
}