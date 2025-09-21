using Application.DTOs.Admin.Attributes;
using Application.DTOs.Admin.Brand;
using Application.DTOs.Admin.Category;
using AutoMapper;
using Domain.Data.Entities;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<Attributes, AttributesDTO>();
            CreateMap<CategoryAttributes, AddAttributeToCategoryDTO>();

            // Brand mappings
            CreateMap<Brand, BrandDTO>();
            CreateMap<UpdateBrandDTO, Brand>();
        }
    }
}