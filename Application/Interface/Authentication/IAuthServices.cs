using Domain.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Authentication
{
    public interface IAuthServices
    {
        Task<string> Register(RegisterDTO model);
    }
}
