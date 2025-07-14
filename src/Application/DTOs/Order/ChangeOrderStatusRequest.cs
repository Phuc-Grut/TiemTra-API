using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Order
{
    public class ChangeOrderStatusRequest
    {
        public OrderStatus NewStatus { get; set; }
    }
}
