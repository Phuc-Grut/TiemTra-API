using Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order, CancellationToken cancellationToken);

        Task<IEnumerable<Order>> GetAllOrder(CancellationToken cancellationToken);

        Task<bool> OrderCodeExistsAsync(string orderCode);

    }
}
