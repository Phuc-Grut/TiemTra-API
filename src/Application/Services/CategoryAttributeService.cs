using Application.DTOs.Attributes;
using Application.Interface;
using AutoMapper;
using Domain.Data.Entities;
using Infrastructure.Interface;
using Shared.Common;
using System.Security.Claims;

namespace Application.Services
{
    public class CategoryAttributeService : ICategoryAttributeService
    {
        private readonly ICategoryAttributesRepository _categoryAttRepo;
        private readonly IAttributesRepository _attributesRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;

        public CategoryAttributeService(ICategoryAttributesRepository categoryAttributesRepository, IMapper mapper, ICategoryRepository categoryRepository, IAttributesRepository attributesRepo)
        {
            _categoryAttRepo = categoryAttributesRepository;
            _mapper = mapper;
            _categoryRepo = categoryRepository;
            _attributesRepo = attributesRepo;
        }

        public async Task SetAttributesForCategory(SetAttributesForCategoryDTO dto, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            var userId = GetUserIdFromClaims.GetUserId(user);

            var existing = await _categoryAttRepo.GetAttributeIdsByCategory(dto.CategoryId, cancellationToken);
            var toAdd = dto.AttributeIds.Except(existing).ToList();
            var toRemove = existing.Except(dto.AttributeIds).ToList();

            //thêm mới
            foreach (var attributeId in toAdd)
            {
                var entry = new CategoryAttributes
                {
                    CategoryId = dto.CategoryId,
                    AttributeId = attributeId,
                };
                await _categoryAttRepo.AddAsync(entry, cancellationToken);
            }
            if (toRemove.Any()) 
            { 
                await _categoryAttRepo.RemoveAsync(dto.CategoryId, toRemove, cancellationToken);
            }
        }
        public async Task<List<int>> GetSelectedAttributeIds(int categoryId, CancellationToken cancellationToken)
        {
            return await _categoryAttRepo.GetAttributeIdsByCategory(categoryId, cancellationToken);
        }
    }
}