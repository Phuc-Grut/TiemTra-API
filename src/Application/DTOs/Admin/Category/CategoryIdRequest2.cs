namespace Application.DTOs.Admin.Category
{
    public class CategoryIdRequest2
    {
        public int CategoryId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}