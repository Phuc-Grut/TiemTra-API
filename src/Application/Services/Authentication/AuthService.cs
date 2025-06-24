using Application.DTOs.Authentication;
using Application.Interface.Authentication;
using Domain.Data.Entities;
using Domain.Enum;
using Domain.Interface;
using Domain.Interface.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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
                RoleId = 1
            };

            await _authRepository.AddUserRole(userRole);
            await _authRepository.SaveChanges();

            var emailMessage = $"Mã xác thực của bạn: <strong>{otp}</strong><br> <br>Mã sẽ hết hạn sau 2 phút.";


            await _emailService.SendEmailAsync(user.Email, "Xác thực tài khoản", emailMessage);

            var resultUser = new UserBasicDTO
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };


            return new ApiResponse(true, "Đăng ký thành công! Vui lòng kiểm tra email để xác thực tài khoản.", resultUser);
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
            var data = await _authRepository.GetUserByEmail(model.Email);
            if (data == null)
                return new ApiResponse(false, "Email không tồn tại");

            if (data.HashPassword != HashPassword(model.Password))
                return new ApiResponse(false, "Mật khẩu không chính xác");

            if (!data.EmailConfirmed)
                return new ApiResponse(false, "Vui lòng xác thực email trước khi đăng nhập");

            if (!string.IsNullOrEmpty(data.RefreshToken))
            {
                data.RefreshToken = null;
                data.RefreshTokenExpiryTime = null;
                await _userRepository.UpdateUser(data, CancellationToken.None);
            }

            string refreshToken;
            var token = GenerateJwtToken(data, out refreshToken);

            data.RefreshToken = refreshToken;
            data.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(3);

            var updateResult = await _userRepository.UpdateUser(data, CancellationToken.None);

            if (!updateResult)
            {
                return new ApiResponse(false, "Đăng nhập thất bại");
            }

            var user = new UserResponseDTO
            {
                //UserId = data.UserId,
                FullName = data.FullName,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber,
                Avatar = data.Avatar,
                Status = data.Status,
                Roles = data.UserRoles?.Select(ur => ur.Role.RoleName).ToList() ?? new List<string>()
            };


            return new ApiResponse(true, "Đăng nhập thành công", user, token, refreshToken);
        }

        private string GenerateJwtToken(User user, out string refreshToken)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var role = user.UserRoles.FirstOrDefault()?.Role?.RoleName ?? "Customer";

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
                expires: DateTime.UtcNow.AddHours(2),
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
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(3);
            var updateResult = await _userRepository.UpdateUser(user, CancellationToken.None);

            if (!updateResult)
            {
                return new ApiResponse(false, "Lỗi hệ thống, không thể cập nhật Refresh Token.");
            }

            return new ApiResponse(true, "Cấp lại token thành công", user, newAccessToken, newRefreshToken);
        }

        public async Task<ApiResponse> ForgotPassword(ForgotPasswordDto dto)
        {
            var user = await _authRepository.GetUserByEmail(dto.Email);
            if (user == null)
                return new ApiResponse(false, "Email không tồn tại");

            var otp = new Random().Next(100000, 999999).ToString();

            user.VerificationCode = otp;
            user.VerificationExpiry = DateTime.UtcNow.AddMinutes(2);

            await _authRepository.SaveChanges();

            var emailMessage = $@"
                <p>Xin chào {user.FullName},</p>
                <p>Bạn vừa yêu cầu đặt lại mật khẩu.</p>
                <p><strong>Mã OTP của bạn là: <span style='color:blue;font-size:18px'>{otp}</span></strong></p>
                <p>Mã sẽ hết hạn sau 2 phút.</p>
                <p>Nếu bạn không thực hiện yêu cầu này, hãy bỏ qua email này.</p>
            ";

            await _emailService.SendEmailAsync(user.Email, "Mã OTP đặt lại mật khẩu", emailMessage);

            return new ApiResponse(true, "Mã OTP đã được gửi đến email của bạn.");
        }

        public async Task<ApiResponse> ResetPassword(ResetPasswordDTO dto)
        {
            var user = await _authRepository.GetUserByEmail(dto.Email);
            if (user == null)
                return new ApiResponse(false, "Email không hợp lệ");

            if (user.VerificationCode != dto.Otp)
                return new ApiResponse(false, "Mã OTP không hợp lệ");

            if (user.VerificationExpiry < DateTime.UtcNow)
                return new ApiResponse(false, "Mã OTP đã hết hạn");

            user.HashPassword = HashPassword(dto.NewPassword);
            user.VerificationCode = null;
            user.VerificationExpiry = null;

            await _authRepository.SaveChanges();

            return new ApiResponse(true, "Mật khẩu đã được đặt lại thành công. Vui lòng đăng nhập lại.");
        }
    }
}