using Domain.Data.Entities;
using Domain.DTOs.Profile;

namespace Domain.Interface
{
    public interface IProfileRepository
    {
        Task<ProfileDto?> GetProfileByUserIdAsync(Guid userId, CancellationToken ct = default);

        Task SaveChangesAsync(CancellationToken ct = default);

        Task<User?> FindUserForUpdateAsync(Guid userId, CancellationToken ct = default);
    }
}