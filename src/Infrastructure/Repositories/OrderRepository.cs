using Domain.Data.Entities;
using Domain.Interface;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddOrderAsync(Order order, CancellationToken cancellationToken)
        {
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetAllOrder(CancellationToken cancellationToken)
        {
            return await _dbContext.Orders
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> OrderCodeExistsAsync(string orderCode)
        {
            return await _dbContext.Orders
                .AsNoTracking()
                .AnyAsync(o => o.OrderCode == orderCode);
        }
    }
}
