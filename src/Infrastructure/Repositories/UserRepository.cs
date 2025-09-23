using Domain.Data.Entities;
using Domain.Interface;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
        }

        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<List<User>> GetUsersByIdsAsync(List<Guid> userIds, CancellationToken cancellationToken)
        {
            return await _context.Users
           .Where(u => userIds.Contains(u.UserId))
           .Select(u => new User
           {
               UserId = u.UserId,
               FullName = u.FullName,
               Email = u.Email
           })
           .ToListAsync(cancellationToken);
        }

        public async Task<bool> UpdateUser(User user, CancellationToken cancellationToken)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> UserCodeExistsAsync(string userCode)
        {
            return await _context.Users.AsNoTracking().AnyAsync(us => us.UserCode == userCode);
        }
    }
}