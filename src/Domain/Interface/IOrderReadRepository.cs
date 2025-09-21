using Domain.DTOs.Dashboard;

namespace Domain.Interface
{
    public interface IOrderReadRepository
    {
        Task<decimal> SumRevenueAsync(DateTime from, DateTime to, CancellationToken ct);

        Task<int> CountOrdersAsync(DateTime from, DateTime to, CancellationToken ct);

        Task<List<DailyPoint>> GetDailySeriesAsync(DateTime from, DateTime to, CancellationToken ct);
    }
}