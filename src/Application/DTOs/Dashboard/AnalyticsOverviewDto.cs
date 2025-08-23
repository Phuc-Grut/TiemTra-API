using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Dashboard
{
    public class AnalyticsOverviewDto
    {
        public long Revenue { get; set; }   
        public double RevenueChange { get; set; }
        public int Orders { get; set; }
        public long Aov { get; set; }
        public double AovChange { get; set; }
        public int NewCustomers { get; set; }

        public double NewCustomersChange { get; set; }
    }
}
