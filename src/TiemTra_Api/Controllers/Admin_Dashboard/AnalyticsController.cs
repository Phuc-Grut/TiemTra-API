using Application.Features.Analytics;
using Application.Interface;
using Domain.DTOs.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace TiemTra_Api.Controllers.Admin_Dashboard
{
    [Route("api/admin/")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _svc;

        public AnalyticsController(IAnalyticsService svc) => _svc = svc;

        [HttpGet("dashboard")]
        public async Task<ActionResult<DashboardResponse>> Get([FromQuery] DashboardRequest req, CancellationToken ct)
        {
            var (from, to) = PeriodHelper.Resolve(req.Period, req.From, req.To, "SE Asia Standard Time");
            var result = await _svc.GetDashboardAsync(from, to, ct);
            return Ok(result);
        }
    }
}