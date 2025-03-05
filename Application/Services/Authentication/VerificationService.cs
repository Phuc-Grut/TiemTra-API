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
                new Claim("email", email),
                new Claim("otp", otp),
                new Claim("exp", DateTime.UtcNow.AddMinutes(2).ToString())
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
    }
}
