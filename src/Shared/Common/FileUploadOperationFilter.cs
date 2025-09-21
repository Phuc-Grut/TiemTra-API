using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Shared.Common
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileParams = context.MethodInfo
                .GetParameters()
                .Where(p =>
                    p.ParameterType == typeof(IFormFile) ||
                    p.ParameterType == typeof(List<IFormFile>));

            if (!fileParams.Any()) return;

            operation.RequestBody = new OpenApiRequestBody
            {
                Content =
        {
            ["multipart/form-data"] = new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Type = "object",
                    Properties = fileParams.ToDictionary(
                        p => p.Name,
                        p => new OpenApiSchema
                        {
                            Type = "string",
                            Format = "binary"
                        }),
                    Required = fileParams.Select(p => p.Name).ToHashSet()
                }
            }
        },
                Required = true // Đảm bảo body là bắt buộc
            };

            // Xóa các tham số cũ nếu có
            operation.Parameters.Clear();
        }
    }
}