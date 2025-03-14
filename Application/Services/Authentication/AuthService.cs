using Application.Interface.Authentication;
using Domain.Data.Entities;
using Application.DTOs.Authentication;
using Infrastructure.Interface.Authentication;
using System.Security.Cryptography;
using System.Text;
using Shared.Common;
using Domain.Enum;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Authentincation
{
    public class AuthService : IAuthServices
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthService(IAuthRepository authRepository, IEmailService emailService, IConfiguration configuration)
        {

            _authRepository = authRepository;
            _configuration = configuration;
            _emailService = emailService;
        }
        // đăng nhập
        public async Task<ApiResponse> Login(LoginDTO model)
        {
            var user = await _authRepository.GetUserByEmail(model.Email);
            if (user == null)
                return new ApiResponse(false, "Email không tồn tại");

            if (user.HashPassword != HashPassword(model.Password))
                return new ApiResponse(false, "Mật khẩu không chính xác");

            if (!user.EmailConfirmed)
                return new ApiResponse(false, "Vui lòng xác thực email trước khi đăng nhập");

            var token = GenerateJwtToken(user);

            return new ApiResponse(true, "Đăng nhập thành công", user, token);
        }

        // Đăng ký tài khoản
        public async Task<ApiResponse> Register(RegisterDTO model)
        {
            if (await _authRepository.EmailExists(model.Email))
                return new ApiResponse(false, "Emaill đã tồn tại");

            var hashedPassword = HashPassword(model.Password);
            var otp = new Random().Next(100000, 999999).ToString();
            bool isFirstUser = !await _authRepository.AnyUserExists();

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                HashPassword = hashedPassword,
                VerificationCode = otp,
                VerificationExpiry = DateTime.UtcNow.AddMinutes(2),
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

            var emailMessage = $"Mã xác thực của bạn: <strong>{otp}</strong><br> <br>Mã sẽ hết hạn sau 2 phút.";

            Console.WriteLine($"otp dã tạo ra: {otp}");


            await _emailService.SendEmailAsync(user.Email, "Xác thực tài khoản", emailMessage);

            return new ApiResponse(true, "Đăng ký thành công! Vui lòng kiểm tra email để xác thực tài khoản.", user);
        }

        //băm password
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        // Xác thực mã OTP
        public async Task<ApiResponse> VerifyOtp(VerifyOtpDTO model)
        {
            var user = await _authRepository.GetUserByEmail(model.Email);
            if (user == null)
                return new ApiResponse(false, "Email không tồn tại");

            if (user.VerificationCode != model.Otp)
                return new ApiResponse(false, "Mã OTP không hợp lệ");

            if (user.VerificationExpiry < DateTime.UtcNow)
                return new ApiResponse(false, "Mã OTP đã hết hạn");

            user.EmailConfirmed = true;
            user.Status = UserStatus.Active;
            user.VerificationCode = null;
            user.VerificationExpiry = null;

            await _authRepository.SaveChanges();

            return new ApiResponse(true, "Xác thực thành công! Tài khoản của bạn đã được kích hoạt");
        }

        // gửi lại otp
        public async Task<ApiResponse> ResendOtp(ResendOtpDTO model)
        {
            var user = await _authRepository.GetUserByEmail(model.Email);
            if (user == null) return new ApiResponse(false, "Email không tồn tại");

            var newOtp = new Random().Next(100000, 999999).ToString();

            user.VerificationCode = newOtp;
            user.VerificationExpiry = DateTime.UtcNow.AddMinutes(2);

            await _authRepository.SaveChanges();

            var emailMessage = $"Mã xác thực mới của bạn: <strong>{newOtp}</strong><br> <br>Mã sẽ hết hạn sau 2 phút.";

            await _emailService.SendEmailAsync(user.Email, "Mã OTP mới", emailMessage);

            return new ApiResponse(true, "Mã OTP mới đã được gửi đến email của bạn.");
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var role = user.UserRoles.FirstOrDefault()?.Role?.RoleName ?? "Customer";

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
