using Application.DTOs.User;
using Domain.Data.Base;

namespace Application.DTOs.Category
{
    public class CategoryDto : BaseEntity
    {
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public int? ParentId { get; set; }
        public string? UpdaterName { get; set; }
        public string? CreatorName { get; set; }
    }
}