using Application.DTOs.Dashboard;
using Domain.DTOs.Dashboard;

namespace Application.Interface
{
    public interface IAnalyticsService
    {
        Task<AnalyticsOverviewDto> GetOverviewAsync(AnalyticsFilterDto filterDto, CancellationToken cancellationToken);

        Task<DashboardResponse> GetDashboardAsync(DateTime from, DateTime to, CancellationToken ct);
    }
}