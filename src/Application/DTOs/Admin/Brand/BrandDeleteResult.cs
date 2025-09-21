namespace Application.DTOs.Admin.Brand
{
    public class BrandDeleteResult
    {
        public int BrandId { get; set; }
        public bool IsDeleted { get; set; }
        public string? Message { get; set; }
    }
}