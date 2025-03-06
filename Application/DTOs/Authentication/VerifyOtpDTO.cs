using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Authentication
{
    public class VerifyOtpDTO
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
}
