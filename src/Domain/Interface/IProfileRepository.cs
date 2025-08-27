using Domain.Data.Entities;
using Domain.DTOs.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IProfileRepository
    {
        Task<ProfileDto?> GetProfileByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
        Task<User?> FindUserForUpdateAsync(Guid userId, CancellationToken ct = default);
    }
}
