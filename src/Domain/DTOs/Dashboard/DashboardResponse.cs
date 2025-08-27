using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Dashboard
{
    public sealed class DashboardResponse
    {
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
        public decimal RevenueChangePct { get; set; }
        public decimal OrderChangePct { get; set; }
        public List<DailyPoint> DailySeries { get; set; } = new();
    }
}
