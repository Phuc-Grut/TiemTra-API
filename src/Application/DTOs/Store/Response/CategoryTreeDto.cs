namespace Application.DTOs.Store.Response
{
    public class CategoryTreeDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<CategoryTreeDto> Children { get; set; } = new();
    }
}