using Domain.Data.Base;

namespace Domain.Data.Entities
{
    public class Role : BaseEntity
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}