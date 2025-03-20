using Application.DTOs.Authentication;
using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Authentication
{
    public interface IAuthServices
    {
        Task<ApiResponse> Register(RegisterDTO model);
        Task<ApiResponse> Login(LoginDTO model);
        Task<ApiResponse> VerifyOtp(VerifyOtpDTO model);
        Task<ApiResponse> ResendOtp(ResendOtpDTO model);
        Task<ApiResponse> RefreshTokenAsync(RefreshTokenDTO model);
    }
}
