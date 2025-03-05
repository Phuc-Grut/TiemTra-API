using Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface.Authentication
{
    public interface IAuthRepository
    {
        Task<User> GetUserByEmail(string email);
        Task<bool> EmailExists(string email);
        public Task AddUserRole(UserRole userRole);
        Task<bool> AnyUserExists();
        Task AddUser(User user);
        Task SaveChanges();
    }
}
