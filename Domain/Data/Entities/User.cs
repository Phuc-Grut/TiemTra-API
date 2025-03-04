using Domain.Data.Base;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Data.Entities
{
    public class User : BaseEntity
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool EmailConfirmed { get; set; } = true;
        public string HashPassword { get; set; }
        public string? PhoneNumber { get; set; }
        public int? Age { get; set; }
        public string? Address { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
