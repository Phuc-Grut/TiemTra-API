namespace Domain.DTOs.Customer
{
    public class CustomerFilterDto
    {
        public string? Keyword { get; set; }
        public string? CustomerCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? SortBy { get; set; }
    }
}