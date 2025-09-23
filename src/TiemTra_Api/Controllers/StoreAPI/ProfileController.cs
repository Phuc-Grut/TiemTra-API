using Application.DTOs;
using Application.Interface;
using Domain.DTOs.Profile;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TiemTra_Api.Controllers.StoreAPI
{
    [Route("api/store/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IProfileServices _services;

        public ProfileController(IFileStorageService fileStorageService, IProfileServices services)
        {
            _fileStorageService = fileStorageService;
            _services = services;
        }

        [HttpPost("add-avatar-user")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadProductImage([FromForm] UploadFileDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("Không có file");

            var url = await _fileStorageService.UploadFileAsync(dto.File, "avatar/");
            return Ok(new { fileUrl = url });
        }

        [HttpGet("get-profile-by-userId")]
        public async Task<IActionResult> GetProfile(CancellationToken ct)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

            var dto = await _services.GetByUserIdAsync(userId, ct);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpPut("edit-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest req, CancellationToken ct)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

            var ok = await _services.UpdateAsync(userId, req, ct);
            return ok ? Ok(new { success = true }) : NotFound();
        }
    }
}