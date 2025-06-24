using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Admin.Cart
{
    public class CartDTO
    {
        public List<CartItemDTO> Items { get; set; } = new();
        public int TotalQuantity => Items.Sum(x => x.Quantity);
        public decimal TotalPrice => Items.Sum(x => x.Price);
    }
}
