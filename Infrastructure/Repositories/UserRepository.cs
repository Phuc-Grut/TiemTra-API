using Domain.Data.Entities;
using Infrastructure.Database;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
               _context = context;
        }

        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
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
            return await _context.SaveChangesAsync(cancellationToken) > 0 ;
        }
    }
}
