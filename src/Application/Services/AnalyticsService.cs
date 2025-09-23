using Application.DTOs.Dashboard;
using Application.Interface;
using Domain.DTOs.Dashboard;
using Domain.Interface;

namespace Application.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IOrderReadRepository _orders;

        public AnalyticsService(IOrderReadRepository order)
        {
            _orders = order;
        }

        public Task<AnalyticsOverviewDto> GetOverviewAsync(AnalyticsFilterDto filterDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<DashboardResponse> GetDashboardAsync(DateTime from, DateTime to, CancellationToken ct)
        {
            // Hiện tại
            var revenue = await _orders.SumRevenueAsync(from, to, ct);
            var orderCount = await _orders.CountOrdersAsync(from, to, ct);
            var grouped = await _orders.GetDailySeriesAsync(from, to, ct);
            var daily = FillMissingDays(from, to, grouped);

            // Kỳ trước
            var prevFrom = from.AddDays(-(to - from).TotalDays);
            var prevTo = from;
            var prevRevenue = await _orders.SumRevenueAsync(prevFrom, prevTo, ct);
            var prevCount = await _orders.CountOrdersAsync(prevFrom, prevTo, ct);

            return new DashboardResponse
            {
                Revenue = revenue,
                OrderCount = orderCount,
                RevenueChangePct = PercentChange(prevRevenue, revenue),
                OrderChangePct = PercentChange(prevCount, orderCount),
                DailySeries = daily
            };
        }

        private static decimal PercentChange(decimal prev, decimal curr)
        => prev == 0 ? (curr > 0 ? 100m : 0m) : Math.Round((curr - prev) / prev * 100m, 1);

        private static List<DailyPoint> FillMissingDays(DateTime from, DateTime to, List<DailyPoint> points)
        {
            var map = points.ToDictionary(p => p.Date, p => p);
            var res = new List<DailyPoint>();
            for (var d = DateOnly.FromDateTime(from.Date); d < DateOnly.FromDateTime(to.Date); d = d.AddDays(1))
                res.Add(map.TryGetValue(d, out var p) ? p : new DailyPoint { Date = d, Revenue = 0, Orders = 0 });
            return res;
        }
    }
}