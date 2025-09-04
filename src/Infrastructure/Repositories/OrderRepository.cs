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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

            // Base query
            var query = _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .AsQueryable();

            // Filters
            if (!string.IsNullOrWhiteSpace(filter.OrderCode))
                query = query.Where(o => o.OrderCode.Contains(filter.OrderCode));

            if (!string.IsNullOrWhiteSpace(filter.CustomerCode))
                query = query.Where(o => o.Customer != null &&
                                         o.Customer.CustomerCode.Contains(filter.CustomerCode));

            if (filter.OrderStatus != 0)
                query = query.Where(o => o.OrderStatus == filter.OrderStatus);

            if (filter.PaymentMethod != 0)
                query = query.Where(o => o.PaymentMethod == filter.PaymentMethod);

            if (filter.PaymentStatus != 0)
                query = query.Where(o => o.PaymentStatus == filter.PaymentStatus);

            if (filter.CreateAt.HasValue)
                query = query.Where(o => o.CreatedAt >= filter.CreateAt.Value);

            var totalItems = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var sortFields = (filter.SortBy ?? "")
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim().ToLower())
                .ToList();

          
                bool isFirstSort = true;

                foreach (var sort in sortFields)
                {
                    if (sort == "createdat-asc")
                    {
                        query = isFirstSort
                            ? query.OrderBy(o => o.CreatedAt)
                            : ((IOrderedQueryable<Order>)query).ThenBy(o => o.CreatedAt);
                        isFirstSort = false;
                    }
                    else if (sort == "createdat-desc")
                    {
                        query = isFirstSort
                            ? query.OrderByDescending(o => o.CreatedAt)
                            : ((IOrderedQueryable<Order>)query).ThenByDescending(o => o.CreatedAt);
                        isFirstSort = false;
                    }
                    else if (sort == "totalamount-asc")
                    {
                        query = isFirstSort
                            ? query.OrderBy(o => o.TotalAmount)
                            : ((IOrderedQueryable<Order>)query).ThenBy(o => o.TotalAmount);
                        isFirstSort = false;
                    }
                    else if (sort == "totalamount-desc")
                    {
                        query = isFirstSort
                            ? query.OrderByDescending(o => o.TotalAmount)
                            : ((IOrderedQueryable<Order>)query).ThenByDescending(o => o.TotalAmount);
                        isFirstSort = false;
                    }
                    // nếu sort key không khớp thì bỏ qua
                }
           

            // Paging + Projection
            var orders = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    OrderCode = o.OrderCode,
                    CustomerName = o.Customer != null ? o.Customer.CustomerName : null,
                    CustomerCode = o.Customer != null ? o.Customer.CustomerCode : null,
                    ReceivertName = o.RecipientName,   // giữ nguyên theo DTO của bạn
                    ReceiverAddress = o.DeliveryAddress,
                    ReceiverPhone = o.ReceiverPhone,
                    TotalAmount = o.TotalAmount,
                    Note = o.Note,
                    OrderStatus = o.OrderStatus,
                    PaymentMethod = o.PaymentMethod,
                    PaymentStatus = o.PaymentStatus,
                    CreateAt = o.CreatedAt,
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

        public async Task<PagedResult<OrderDto>> GetByUserIDAsync(Guid userID, OrderFillterDto filter, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            IQueryable<Order> query = _dbContext.Orders
                   .AsNoTracking()
                   .Where(o => o.Customer != null && o.Customer.UserId == userID );

            //// Filters
            //if (!string.IsNullOrWhiteSpace(filter.OrderCode))
            //    query = query.Where(o => o.OrderCode.Contains(filter.OrderCode));
            //if (!string.IsNullOrWhiteSpace(filter.CustomerCode))
            //    query = query.Where(o => o.Customer != null &&
            //                             o.Customer.CustomerCode.Contains(filter.CustomerCode));
            //if (filter.OrderStatus != 0)
            //    query = query.Where(o => o.OrderStatus == filter.OrderStatus);
            //if (filter.PaymentMethod != 0)
            //    query = query.Where(o => o.PaymentMethod == filter.PaymentMethod);
            //if (filter.PaymentStatus != 0)
            //    query = query.Where(o => o.PaymentStatus == filter.PaymentStatus);
            if (filter.CreateAt.HasValue)
                query = query.Where(o => o.CreatedAt >= filter.CreateAt.Value);

            var totalItems = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var sortFields = (filter.SortBy ?? "")
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim().ToLower())
                .ToList();


            bool isFirstSort = true;

            foreach (var sort in sortFields)
            {
                if (sort == "createdat-asc")
                {
                    query = isFirstSort
                    ? query.OrderBy(o => o.CreatedAt)
                        : ((IOrderedQueryable<Order>)query).ThenBy(o => o.CreatedAt);
                    isFirstSort = false;
                }
                else if (sort == "createdat-desc")
                {
                    query = isFirstSort
                    ? query.OrderByDescending(o => o.CreatedAt)
                        : ((IOrderedQueryable<Order>)query).ThenByDescending(o => o.CreatedAt);
                    isFirstSort = false;
                }
                else if (sort == "totalamount-asc")
                {
                    query = isFirstSort
                    ? query.OrderBy(o => o.TotalAmount)
                        : ((IOrderedQueryable<Order>)query).ThenBy(o => o.TotalAmount);
                    isFirstSort = false;
                }
                else if (sort == "totalamount-desc")
                {
                    query = isFirstSort
                        ? query.OrderByDescending(o => o.TotalAmount)
                        : ((IOrderedQueryable<Order>)query).ThenByDescending(o => o.TotalAmount);
                    isFirstSort = false;
                }
                // nếu sort key không khớp thì bỏ qua
            }


            // Paging + Projection
            var orders = await query
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    OrderCode = o.OrderCode,
                    CustomerName = o.Customer != null ? o.Customer.CustomerName : null,
                    CustomerCode = o.Customer != null ? o.Customer.CustomerCode : null,
                    ReceivertName = o.RecipientName,   // giữ nguyên theo DTO của bạn
                    ReceiverAddress = o.DeliveryAddress,
                    ReceiverPhone = o.ReceiverPhone,
                    TotalAmount = o.TotalAmount,
                    Note = o.Note,
                    OrderStatus = o.OrderStatus,
                    PaymentMethod = o.PaymentMethod,
                    PaymentStatus = o.PaymentStatus,
                    ShippingFee = o.ShippingFee,
                    CreateAt = o.CreatedAt,
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
    }
}
