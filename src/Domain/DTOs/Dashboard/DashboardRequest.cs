using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Dashboard
{
    public sealed class DashboardRequest
    {
        /// <summary>
        /// Preset khoảng thời gian: Today, Yesterday, Last7Days, Last30Days, ThisMonth, LastMonth, ThisQuarter, Custom
        /// </summary>
        public PeriodPreset Period { get; set; } = PeriodPreset.ThisMonth;

        /// <summary>
        /// Nếu Period = Custom thì phải truyền From/To
        /// </summary>
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
