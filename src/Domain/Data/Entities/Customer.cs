using Domain.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Data.Entities
{
    public class Customer : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string RecipientName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public Guid? UserId { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
