using Domain.Data.Entities;
using Domain.DTOs.Profile;
using Domain.Enum;
using Domain.Interface;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly AppDbContext _db;
        public ProfileRepository(AppDbContext db) => _db = db;

        public async Task<ProfileDto?> GetProfileByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _db.Users
                .AsNoTracking()
                .Where(u => u.UserId == userId && u.Status == UserStatus.Active)
                .Select(u => new ProfileDto
                {
                    UserId = u.UserId,
                    UserCode = u.UserCode,
                    Email = u.Email,
                    FullName = u.FullName,
                    PhoneNumber = u.PhoneNumber ?? (u.Customer != null ? u.Customer.PhoneNumber : null),
                    Age = u.Age,
                    Address = u.Address ?? (u.Customer != null ? u.Customer.Address : null),
                    Avatar = u.Avatar,
                })
                .FirstOrDefaultAsync(ct);
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);

        public async Task<User?> FindUserForUpdateAsync(Guid userId, CancellationToken ct = default)
        {
            return await _db.Users
                .FirstOrDefaultAsync(u => u.UserId == userId && u.Status == UserStatus.Active, ct);
        }
    }
}
