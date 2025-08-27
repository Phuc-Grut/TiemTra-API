// Application/Features/Analytics/PeriodHelper.cs
using Domain.DTOs.Dashboard;
using System;

namespace Application.Features.Analytics;

public static class PeriodHelper
{
    public static (DateTime from, DateTime to) Resolve(
        PeriodPreset preset, DateTime? from, DateTime? to, string tz)
    {
        var tzInfo = TimeZoneInfo.FindSystemTimeZoneById(tz);
        var now = TimeZoneInfo.ConvertTime(DateTime.UtcNow, tzInfo);

        DateTime start, end;
        switch (preset)
        {
            case PeriodPreset.Today:
                start = now.Date; end = start.AddDays(1); break;
            case PeriodPreset.Yesterday:
                end = now.Date; start = end.AddDays(-1); break;
            case PeriodPreset.Last7Days:
                end = now.Date.AddDays(1); start = end.AddDays(-7); break;
            case PeriodPreset.Last30Days:
                end = now.Date.AddDays(1); start = end.AddDays(-30); break;
            case PeriodPreset.ThisMonth:
                start = new DateTime(now.Year, now.Month, 1); end = start.AddMonths(1); break;
            case PeriodPreset.LastMonth:
                end = new DateTime(now.Year, now.Month, 1); start = end.AddMonths(-1); break;
            case PeriodPreset.ThisQuarter:
                int q = (now.Month - 1) / 3;
                start = new DateTime(now.Year, q * 3 + 1, 1); end = start.AddMonths(3); break;
            case PeriodPreset.Custom:
            default:
                if (from is null || to is null) throw new ArgumentException("from/to required for Custom");
                start = from.Value; end = to.Value; break;
        }

        // Trả về UTC nếu DB lưu UTC
        return (TimeZoneInfo.ConvertTimeToUtc(start, tzInfo),
                TimeZoneInfo.ConvertTimeToUtc(end, tzInfo));
    }
}
