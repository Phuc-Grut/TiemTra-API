using Domain.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Data.Entities
{
    public class CartItem : BaseEntity
    {
        public Guid CartItemId { get; set; }
        public Guid CartId { get; set; }
        public Guid? ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public Cart Cart { get; set; }
        public Product? Product { get; set; }
    }
}
