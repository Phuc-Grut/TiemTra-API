using Application.Interface.Authentication;
using Domain.DTOs.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APISell.Controllers.Authentication
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO model)
        {
            var result = await _authServices.Register(model);
            return Ok(new {message = result});
        }
    }
}
