using Application.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IAnalyticsService
    {
        Task<AnalyticsOverviewDto> GetOverviewAsync (AnalyticsFilterDto filterDto, CancellationToken cancellationToken);
    }
}
