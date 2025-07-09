using Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Order
{
    public class CreateOrderItemDto
    {
        public Guid ProductId { get; set; }
        public Guid? ProductVariationId { get; set; }
        public int Quantity { get; set; }
    }
}
