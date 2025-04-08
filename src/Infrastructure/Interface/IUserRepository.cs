using Domain.Data.Entities;

namespace Infrastructure.Interface
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersByIdsAsync(List<Guid> userIds, CancellationToken cancellationToken);

        Task<bool> UpdateUser(User user, CancellationToken cancellationToken);

        Task<User> GetUserByRefreshToken(string refreshToken);
    }
}