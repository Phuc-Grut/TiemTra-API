using Domain.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Data.Entities
{
    public class Cart : BaseEntity
    {
        public Guid CartId { get; set; }
        public Guid? UserId { get; set; }

        public User? User { get; set; }

        public virtual ICollection<CartItem> CartItem { get; set; } = new List<CartItem>();

        public int TotalItems { get; set; }

        public decimal TotalPrice { get; set; }

    }
}
