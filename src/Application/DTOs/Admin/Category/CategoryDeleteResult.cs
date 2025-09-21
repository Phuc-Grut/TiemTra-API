namespace Application.DTOs.Admin.Category
{
    public class CategoryDeleteResult
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}