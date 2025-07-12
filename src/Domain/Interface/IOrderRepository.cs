using Domain.Data.Entities;
using Domain.DTOs;
using Domain.DTOs.Order;
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


        Task<PagedResult<OrderDto>> GetPagedOrdersAsync(OrderFillterDto filter, int pageNumber, int pageSize, CancellationToken cancellationToken);
    }
}
