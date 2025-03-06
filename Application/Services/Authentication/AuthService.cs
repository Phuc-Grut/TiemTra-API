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
using Domain.Enum;

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

            bool isFirstUser = !await _authRepository.AnyUserExists();

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                HashPassword = hashedPassword
            };

            await _authRepository.AddUser(user);
            await _authRepository.SaveChanges();

            var userRole = new UserRole
            {
                UserId = user.UserId,
                RoleId = isFirstUser ? 1 : 3
            };

            await _authRepository.AddUserRole(userRole);
            await _authRepository.SaveChanges();

            var otp = new Random().Next(100000, 999999).ToString();
            var token = VerificationService.GenerateVerificationToken(user.Email, otp);
            var emailMessage = $"Mã xác thực của bạn: <strong>{otp}</strong><br>Mã JWT: <strong>{token}</strong><br>Mã sẽ hết hạn sau 2 phút.";

            Console.WriteLine($"otp dã tạo ra: {otp}");
            Console.WriteLine($"JWT OTP dã tạo ra: {token}");
            

            await _emailService.SendEmailAsync(user.Email, "Xác thực tài khoản", emailMessage);

            return new ApiResponse(true, "Đăng ký thành công! Vui lòng kiểm tra email để xác thực tài khoản.", user);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        public async Task<ApiResponse> VerifyOtp(VerifyOtpDTO model)
        {
            var user = await _authRepository.GetUserByEmail(model.Email);
            if (user == null)
            {
                Console.WriteLine("Email không tồn tại");
                return new ApiResponse(false, "Email không tồn tại.");
            }

            Console.WriteLine($"User tìm thấy: {user.Email}");

            // Kiểm tra OTP có hợp lệ
            bool isValidOtp = VerificationService.ValidateVerificationToken(user.Email, model.Otp);
            if (!isValidOtp)
            {
                Console.WriteLine($"❌ OTP không hợp lệ hoặc đã hết hạn: {model.Otp}");
                return new ApiResponse(false, "Mã OTP không hợp lệ hoặc đã hết hạn.");
            }

            Console.WriteLine("OTP hợp lệ, cập nhật tài khoản.");
            // Cập nhật trạng thái tài khoản
            user.EmailConfirmed = true;
            user.Status = UserStatus.Active;
            user.UpdatedAt = DateTime.UtcNow;

            await _authRepository.SaveChanges();

            return new ApiResponse(true, "Xác thực thành công! Tài khoản đã được kích hoạt.");
        }
    }
}
