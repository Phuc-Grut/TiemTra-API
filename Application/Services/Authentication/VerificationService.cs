using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Authentication
{
    public class VerificationService
    {
        private const string SecretKey = "wuffpzSXWzNqb8PuG7v8dPwwcgtNl/le4iYp54HVv4c=";

        // Tạo JWT Token chứa mã OTP
        public static string GenerateVerificationToken(string email, string otp)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim("otp", otp),
                //new Claim("exp", DateTime.UtcNow.AddMinutes(2).ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "https://grunt-app.com",
                audience: "https://grunt-api.com",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static bool ValidateVerificationToken(string email, string otpToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(SecretKey); // Key bảo mật

            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = "https://grunt-app.com",
                    ValidAudience = "https://grunt-api.com",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(otpToken, tokenValidationParameters, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;

                var tokenEmail = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var tokenOtp = jwtToken.Claims.FirstOrDefault(c => c.Type == "otp")?.Value;

                Console.WriteLine($"Giải mã JWT: Email = {tokenEmail}, OTP = {tokenOtp}");

                return tokenEmail == email && tokenOtp == otpToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi khi giải mã OTP: {ex.Message}");
                return false;
            }
        }

    }
}
