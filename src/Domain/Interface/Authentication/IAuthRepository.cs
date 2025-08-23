using Domain.Data.Entities;

namespace Domain.Interface.Authentication
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