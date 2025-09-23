namespace Application.DTOs.Admin.Attributes
{
    public class SetAttributesForCategoryDTO
    {
        public int CategoryId { get; set; }
        public List<int> AttributeIds { get; set; } = new();
    }
}