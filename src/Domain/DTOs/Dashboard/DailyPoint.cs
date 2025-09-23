namespace Domain.DTOs.Dashboard
{
    public sealed class DailyPoint
    {
        public DateOnly Date { get; set; }
        public decimal Revenue { get; set; }
        public int Orders { get; set; }
    }
}