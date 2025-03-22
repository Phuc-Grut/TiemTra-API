﻿using Application.DTOs.User;
using Domain.Data.Base;

namespace Application.DTOs.Attributes
{
    public class AttributesDTO : BaseEntity
    {
        public int AttributeId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public UserDTO? Creator { get; set; }
        public UserDTO? Updater { get; set; }
    }
}