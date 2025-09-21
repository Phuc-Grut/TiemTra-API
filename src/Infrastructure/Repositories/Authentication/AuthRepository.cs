using Domain.Data.Entities;
using Domain.Interface.Authentication;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Authentication
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _context.Users.AnyAsync(us => us.Email == email);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(us => us.Email == email);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyUserExists()
        {
            return await _context.Users.AnyAsync();
        }

        public async Task AddUserRole(UserRole userRole)
        {
            await _context.UserRoles.AddAsync(userRole);
        }

        public async Task<bool> PhoneNumberExists(string phoneNumber)
        {
            return await _context.Users.AnyAsync(us => us.PhoneNumber == phoneNumber);
        }

        public async Task<bool> UserCodeExistsAsync(string userCode)
        {
            return await _context.Users.AsNoTracking().AnyAsync(c => c.UserCode == userCode);
        }
    }
}