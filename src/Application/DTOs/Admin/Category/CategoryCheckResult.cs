namespace Application.DTOs.Admin.Category
{
    public class CategoryCheckResult
    {
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; } = string.Empty;
        public bool CanDelete { get; set; }
        public string? Message { get; set; } = string.Empty;
    }
}