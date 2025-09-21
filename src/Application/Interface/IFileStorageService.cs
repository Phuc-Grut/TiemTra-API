using Microsoft.AspNetCore.Http;

namespace Application.Interface
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string folder);
    }
}