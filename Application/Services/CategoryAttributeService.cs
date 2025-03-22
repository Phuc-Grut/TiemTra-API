using Application.DTOs.Category;
using Application.Interface;
using AutoMapper;
using Domain.Data.Entities;
using Infrastructure.Interface;
using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<AddAttributeToCategoryDTO> AddAttributesToCategory(AddAttributeToCategoryDTO addDto, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            var userId = GetUserIdFromClaims.GetUserId(user);
            var hasChildren = await _categoryRepo.HasChildCategories(addDto.CategoryId);

            var categoryExists = await _categoryRepo.CategoryExists(addDto.CategoryId);
            var attributeExists = await _attributesRepo.AttributeExists(addDto.AttributesId);

            if (!categoryExists)
                    throw new Exception("Danh mục không tồn tại.");

            if (!attributeExists)
                throw new Exception("Thuộc tính không tồn tại.");

            if (hasChildren)
            {
                throw new Exception("Danh mục này có danh mục con.");
            }

            if (await _categoryAttRepo.ExistsAsync(addDto.CategoryId, addDto.AttributesId))
                throw new Exception("Thuộc tính đã tồn tại trong danh mục.");

            var newCtgrAtb = new CategoryAttributes
            {
                CategoryId = addDto.CategoryId,
                AttributeId = addDto.AttributesId,
                UpdatedAt = DateTime.Now,
                UpdatedBy = userId,
                CreatedBy = userId,

            };

            var rs = _categoryAttRepo.AddAttributesToCategory(newCtgrAtb, cancellationToken);

            return new AddAttributeToCategoryDTO
            {
                CategoryId = newCtgrAtb.CategoryId,
                AttributesId = newCtgrAtb.AttributeId
            };
        }

        public async Task<List<AddAttributeToCategoryDTO>> GetAttributesByCategory(int categoryId)
        {
            var attributes = await _categoryAttRepo.GetAttributesByCategory(categoryId);
            return _mapper.Map<List<AddAttributeToCategoryDTO>>(attributes);
        }

        public Task RemoveAttributesToCategory(int categoryId, int attributeId)
        {
            throw new NotImplementedException();
        }
    }
}
