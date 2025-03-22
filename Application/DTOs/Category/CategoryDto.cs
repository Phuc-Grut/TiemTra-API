using Application.DTOs.User;
using Domain.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Category
{
    public class CategoryDto : BaseEntity
    {
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public int? ParentId { get; set; }
        public UserDTO? Creator { get; set; }
        public UserDTO? Updater { get; set; }
    }
}
