using Domain.Data.Base;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Data.Entities
{
    public class User : BaseEntity
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public string HashPassword { get; set; }
        public string? PhoneNumber { get; set; }
        public int? Age { get; set; }
        public string? Address { get; set; }
        public string? VerificationCode { get; set; } // Mã OTP
        public DateTime? VerificationExpiry { get; set; } // thời gian hết hạn otp
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string? Avatar { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Pending;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
