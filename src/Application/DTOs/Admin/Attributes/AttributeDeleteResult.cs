namespace Application.DTOs.Admin.Attributes
{
    public class AttributeDeleteResult
    {
        public int AttributeId { get; set; }
        public string AttributeName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}