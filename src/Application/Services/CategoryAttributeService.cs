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

            var hasChildren = await _categoryRepo.HasChildCategories(dto.CategoryId, cancellationToken);
            if (hasChildren)
            {
                throw new InvalidOperationException("Không thể thêm thuộc tính cho danh mục cha có danh mục con.");
            }

            var existing = await _categoryAttRepo.GetAttributeIdsByCategory(dto.CategoryId, cancellationToken);
            var request = dto.AttributeIds.Distinct().ToList();

            var toRemove = existing.Intersect(request).ToList();

            var toAdd = request.Except(existing).ToList();

            foreach (var attributeId in toAdd)
            {
                var entry = new CategoryAttributes
                {
                    CategoryId = dto.CategoryId,
                    AttributeId = attributeId,
                    CreatedAt = DateTime.Now,
                    CreatedBy = userId
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