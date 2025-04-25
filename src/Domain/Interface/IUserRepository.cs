using Domain.Data.Entities;

namespace Domain.Interface
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersByIdsAsync(List<Guid> userIds, CancellationToken cancellationToken);

        Task<bool> UpdateUser(User user, CancellationToken cancellationToken);

        Task<User> GetUserByRefreshToken(string refreshToken);
    }
}