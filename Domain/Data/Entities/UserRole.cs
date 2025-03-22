namespace Domain.Data.Entities
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }
}