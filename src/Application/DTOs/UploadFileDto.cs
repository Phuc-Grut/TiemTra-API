using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.DTOs
{
    public class UploadFileDto
    {
        [FromForm]
        public IFormFile File { get; set; }
    }
}