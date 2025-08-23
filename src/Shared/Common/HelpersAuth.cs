using System;
using System.Security.Cryptography;
using System.Text;
// nếu muốn dùng rút gọn: using BCrypt.Net;

namespace Shared.Common
{
    /// Helper tạo OTP số an toàn & hash/verify OTP bằng BCrypt.
    public static class HelpersAuth
    {
        public static string GenerateNumericOtp(int length = 6)
        {
            if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            var sb = new StringBuilder(length);
            foreach (var b in bytes) sb.Append((b % 10).ToString());
            return sb.ToString();
        }

        public static string HashOtp(string otp, int workFactor = 10)
        {
            if (string.IsNullOrWhiteSpace(otp)) throw new ArgumentNullException(nameof(otp));
            return BCrypt.Net.BCrypt.HashPassword(otp, workFactor); // hoặc: BCrypt.HashPassword(otp, workFactor);
        }

        public static bool VerifyOtp(string inputOtp, string otpHash)
        {
            if (string.IsNullOrWhiteSpace(inputOtp)) return false;
            if (string.IsNullOrWhiteSpace(otpHash)) return false;
            return BCrypt.Net.BCrypt.Verify(inputOtp, otpHash); // hoặc: BCrypt.Verify(inputOtp, otpHash);
        }

        public static string GenerateAndHashOtp(out string otp, int length = 6, int workFactor = 10)
        {
            otp = GenerateNumericOtp(length);
            return HashOtp(otp, workFactor);
        }

        public static DateTime CreateExpiryUtc(int minutes = 10)
        {
            if (minutes <= 0) throw new ArgumentOutOfRangeException(nameof(minutes));
            return DateTime.UtcNow.AddMinutes(minutes);
        }

        public static bool IsExpired(DateTime? expiryUtc) =>
            !expiryUtc.HasValue || expiryUtc.Value < DateTime.UtcNow;
    }
}
