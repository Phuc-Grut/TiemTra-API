using Application.Interface.Authentication;
using Domain.Data.Entities;
using Domain.DTOs.Authentication;
using Infrastructure.Interface.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Authentincation
{
    public class AuthService : IAuthServices
    {
        private readonly IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async Task<string> Register(RegisterDTO model)
        {
            if (await _authRepository.EmailExists(model.Email))
                return "Email đã tồn tại";
            
            var hashedPassword = HashPassword(model.Password);

            var user = new User
            {
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                HashPassword = hashedPassword
            };

            await _authRepository.AddUser(user);
            await _authRepository.SaveChanges();
            
            return "Đăng ký thành công, mã xác nhận đã được gửi đến mail của bạn!";
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
