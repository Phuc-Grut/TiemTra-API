namespace Application.DTOs.Admin.Product
{
    public class BulkSoftDeleteRequest
    {
        public List<Guid> Ids { get; set; } = new();
    }
}