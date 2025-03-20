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
using Infrastructure.Interface;

namespace Application.Services.Authentincation
{
    public class AuthService : IAuthServices
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public AuthService(IAuthRepository authRepository, IEmailService emailService, IConfiguration configuration, IUserRepository userRepository)
        {

            _authRepository = authRepository;
            _configuration = configuration;
            _emailService = emailService;
            _userRepository = userRepository;
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

        public async Task<ApiResponse> Login(LoginDTO model)
        {
            var user = await _authRepository.GetUserByEmail(model.Email);
            if (user == null)
                return new ApiResponse(false, "Email không tồn tại");

            if (user.HashPassword != HashPassword(model.Password))
                return new ApiResponse(false, "Mật khẩu không chính xác");

            if (!user.EmailConfirmed)
                return new ApiResponse(false, "Vui lòng xác thực email trước khi đăng nhập");

            // ✅ Kiểm tra nếu user đã có refresh token cũ => thu hồi token cũ (có thể làm nếu muốn tăng bảo mật)
            if (!string.IsNullOrEmpty(user.RefreshToken))
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await _userRepository.UpdateUser(user, CancellationToken.None);
            }

            // ✅ Tạo Access Token & Refresh Token
            string refreshToken;
            var token = GenerateJwtToken(user, out refreshToken);

            // ✅ Lưu Refresh Token vào database
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(3);

            var updateResult = await _userRepository.UpdateUser(user, CancellationToken.None);
            if (!updateResult)
            {
                return new ApiResponse(false, "Đăng nhập thất bại");
            }

            return new ApiResponse(true, "Đăng nhập thành công", user, token, refreshToken);
        }


        private string GenerateJwtToken(User user, out string refreshToken)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var role = user.UserRoles.FirstOrDefault()?.Role?.RoleName ?? "Customer";

            // ✅ Tạo một GUID mới cho Refresh Token
            refreshToken = GenerateRefreshToken();
            var refreshTokenId = Guid.NewGuid().ToString();

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, refreshTokenId)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }

        public async Task<ApiResponse> RefreshTokenAsync(RefreshTokenDTO model)
        {
            var user = await _userRepository.GetUserByRefreshToken(model.RefreshToken);

            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                return new ApiResponse(false, "Refresh Token hết hạn. Vui lòng đăng nhập lại.");
            }

            string newRefreshToken;
            var newAccessToken = GenerateJwtToken(user, out newRefreshToken);

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(10);
            var updateResult = await _userRepository.UpdateUser(user, CancellationToken.None);

            if (!updateResult)
            {
                return new ApiResponse(false, "Lỗi hệ thống, không thể cập nhật Refresh Token.");
            }

            return new ApiResponse(true, "Cấp lại token thành công", user, newAccessToken, newRefreshToken);
        }
    }                                                                                                       
}
