using Application.DTOs.Attributes;
using Application.DTOs.Category;
using AutoMapper;
using Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<Attributes, AttributesDTO>();
        }
    }
}
