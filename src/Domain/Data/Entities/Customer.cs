using Domain.Data.Base;

namespace Domain.Data.Entities
{
    public class Customer : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public Guid? UserId { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public string? AvatarUrl { get; set; }
    }
}