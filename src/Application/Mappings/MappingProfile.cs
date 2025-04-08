using Application.DTOs.Attributes;
using Application.DTOs.Category;
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
        }
    }
}