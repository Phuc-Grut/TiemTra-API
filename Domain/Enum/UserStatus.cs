namespace Domain.Enum
{
    public enum UserStatus
    {
        Pending = 0,  // Chờ xác nhận email
        Active = 1,   // Đang hoạt động
        Suspended = 2, // Bị khóa
        Banned = 3 // Cấm vĩnh viễn
    }
}