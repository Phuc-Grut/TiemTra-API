using Domain.DTOs.Profile;

namespace Application.Interface
{
    public interface IProfileServices
    {
        Task<ProfileDto?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);

        Task<bool> UpdateAsync(Guid userId, UpdateProfileRequest req, CancellationToken ct = default);
    }
}