using Application.Interface.Authentication;
using Domain.Data.Entities;
using Application.DTOs.Authentication;
using Infrastructure.Interface.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Shared.Common;
using Application.Services.Authentication;

namespace Application.Services.Authentincation
{
    public class AuthService : IAuthServices
    {
        private readonly IAuthRepository _authRepository;

        private readonly IEmailService _emailService;

        public AuthService(IAuthRepository authRepository, IEmailService emailService)
        {
            _authRepository = authRepository;
            _emailService = emailService;
        }
        public async Task<ApiResponse> Register(RegisterDTO model)
        {
            if (await _authRepository.EmailExists(model.Email))
                return new ApiResponse(false, "Emaill đã tồn tại");
            
            var hashedPassword = HashPassword(model.Password);

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                HashPassword = hashedPassword
            };

            await _authRepository.AddUser(user);
            await _authRepository.SaveChanges();

            var otp = new Random().Next(100000, 999999).ToString();

            var token = VerificationService.GenerateVerificationToken(user.Email, otp);
            var emailMessage = $"Mã xác thực của bạn: <strong>{otp}</strong>. <br>Mã sẽ hết hạn sau 2 phút.";

            await _emailService.SendEmailAsync(user.Email, "Xác thực tài khoản", emailMessage);

            return new ApiResponse(true, "Đăng ký thành công! Vui lòng kiểm tra email để xác thực tài khoản.", token);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
