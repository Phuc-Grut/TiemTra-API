using Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ICustomerService
    {
        Task<Guid> GetOrCreateCustomerAsync(CreateOrderRequest request, Guid? userId, CancellationToken cancellationToken);
    }
}
