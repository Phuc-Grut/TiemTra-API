using Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersByIdsAsync(List<Guid> userIds, CancellationToken cancellationToken);
        Task<bool> UpdateUser(User user, CancellationToken cancellationToken);
        Task<User> GetUserByRefreshToken(string refreshToken);
    }
}
