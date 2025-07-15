using Domain.Data.Entities;
using Domain.DTOs;
using Domain.DTOs.Order;
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

        public async Task<Order?> GetByIdWithItemsAsync(Guid orderId, CancellationToken cancellationToken)
        {
            return await _dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariations)
                .FirstOrDefaultAsync(o => o.OrderId == orderId, cancellationToken);
        }


        public async Task<PagedResult<OrderDto>> GetPagedOrdersAsync(OrderFillterDto filter, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = _dbContext.Orders.Include(o => o.Customer).AsQueryable();

            if (!string.IsNullOrEmpty(filter.OrderCode))
            {
                query = query.Where(o => o.OrderCode.Contains(filter.OrderCode));
            }

            if (!string.IsNullOrEmpty(filter.CustomerCode))
            {
                query = query.Where(o => o.Customer.CustomerCode.Contains(filter.CustomerCode));
            }

            if (filter.OrderStatus != 0)
                query = query.Where(o => o.OrderStatus == filter.OrderStatus);

            if (filter.PaymentMethod != 0)
                query = query.Where(o => o.PaymentMethod == filter.PaymentMethod);

            if (filter.PaymentStatus != 0)
                query = query.Where(o => o.PaymentStatus == filter.PaymentStatus);

            if (filter.CreateAt.HasValue)
                query = query.Where(o => o.CreatedAt >= filter.CreateAt.Value);

            if (filter.UpdateAt.HasValue)
                query = query.Where(o => o.UpdatedAt >= filter.UpdateAt.Value);

            var totalItems = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var orders = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    OrderCode = o.OrderCode,
                    CustomerName = o.Customer.CustomerName,
                    CustomerCode = o.Customer.CustomerCode,
                    ReceivertName = o.RecipientName,
                    ReceiverAddress = o.DeliveryAddress,
                    ReceiverPhone = o.ReceiverPhone,
                    TotalAmount = o.TotalAmount,
                    Note = o.Note,
                    OrderStatus = o.OrderStatus,
                    PaymentMethod = o.PaymentMethod,
                    PaymentStatus = o.PaymentStatus,
                    CreateAt = o.CreatedAt,
                    UpdateAt = o.UpdatedAt
                    
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<OrderDto>
            {
                Items = orders,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };

        }

        public async Task<bool> OrderCodeExistsAsync(string orderCode)
        {
            return await _dbContext.Orders
                .AsNoTracking()
                .AnyAsync(o => o.OrderCode == orderCode);
        }

        public async Task UpdateAsync(Order order, CancellationToken cancellationToken)
        {
            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Order> GetByIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            return await _dbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId, cancellationToken);
        }
    }
}
