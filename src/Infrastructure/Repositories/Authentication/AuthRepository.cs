﻿using Domain.Data.Entities;
using Infrastructure.Database;
using Domain.Interface.Authentication;
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
            return await _context.Users.FirstOrDefaultAsync(us => us.Email == email);
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
    }
}