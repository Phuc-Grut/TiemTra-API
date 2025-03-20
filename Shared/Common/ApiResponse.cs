namespace Shared.Common
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }

        public ApiResponse(bool success, string message, object? data = null, string? token = null, string? refreshToken = null)
        {
            Success = success;
            Message = message;
            Data = data;
            Token = token;
            RefreshToken = refreshToken;
        }
    }
}
