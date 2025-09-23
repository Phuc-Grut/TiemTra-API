// Infrastructure/Repositories/OrderReadRepository.cs
using Domain.Data.Entities;
using Domain.DTOs.Dashboard;
using Domain.Enum;
using Domain.Interface;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderReadRepository : IOrderReadRepository
{
    private readonly AppDbContext _db;

    public OrderReadRepository(AppDbContext db) => _db = db;

    public async Task<int> CountOrdersAsync(DateTime from, DateTime to, CancellationToken ct)
        => await Base(from, to).CountAsync(ct);

    public async Task<List<DailyPoint>> GetDailySeriesAsync(DateTime from, DateTime to, CancellationToken ct)
    {
        var raw = await Base(from, to)
            .GroupBy(o => o.CreatedAt.Date)
            .Select(g => new
            {
                Day = g.Key,
                Revenue = g.Sum(x => x.TotalAmount),
                Orders = g.Count()
            })
            .ToListAsync(ct);

        return raw.Select(x => new DailyPoint
        {
            Date = DateOnly.FromDateTime(x.Day),
            Revenue = x.Revenue,
            Orders = x.Orders
        }).ToList();
    }

    public async Task<decimal> SumRevenueAsync(DateTime from, DateTime to, CancellationToken ct)
        => await Base(from, to).SumAsync(o => (decimal?)o.TotalAmount, ct) ?? 0m;

    private IQueryable<Order> Base(DateTime from, DateTime to)
    {
        var q = _db.Orders.AsNoTracking()
            .Where(o => o.CreatedAt >= from && o.CreatedAt < to)
            .Where(o => o.OrderStatus == OrderStatus.Delivered || o.PaymentStatus == PaymentStatus.Paid);

        return q;
    }
}