namespace Domain.DTOs.Customer
{
    public class CustomerDto
    {
        public string? Avatar { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string? CustomerName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public int OrderSuccessful { get; set; }
    }
}