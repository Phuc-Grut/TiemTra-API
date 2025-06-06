namespace Application.DTOs.Admin.Category
{
    public class UpCategoryDto
    {
        public string? CategoryName { get; set; }
        public int? ParentId { get; set; }
        public string? Description { get; set; }
    }
}