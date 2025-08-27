using Domain.DTOs.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IProfileServices
    {
        Task<ProfileDto?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task<bool> UpdateAsync(Guid userId, UpdateProfileRequest req, CancellationToken ct = default);
    }
}
