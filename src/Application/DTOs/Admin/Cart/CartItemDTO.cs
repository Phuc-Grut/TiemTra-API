
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Admin.Cart
{
    public class CartItemDTO
    {
        public Guid CartItemId { get; set; }
        public Guid ProductId { get; set; }
        public Guid ProductVariationId { get; set; }
        public string ProductCode { get; set; }
        public string? ProductName { get; set; }
        public string? ProductVariationName { get; set; }
        public string? PreviewImage { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
