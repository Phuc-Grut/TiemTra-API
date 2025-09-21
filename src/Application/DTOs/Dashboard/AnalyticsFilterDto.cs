namespace Application.DTOs.Dashboard
{
    public class AnalyticsFilterDto
    {
        public DateTime From { get; set; }     // local time (Asia/Ho_Chi_Minh)
        public DateTime To { get; set; }       // local time (inclusive)
        public string? Channel { get; set; }
    }
}