using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Common
{
    public class UploadImageToAzure
    {
        //private async Task<string> UploadImageToAzure(IFormFile file)
        //{
        //    var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        //    await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        //    string originalName = Path.GetFileNameWithoutExtension(file.FileName);
        //    string extension = Path.GetExtension(file.FileName);
        //    string blobName = $"{originalName}{extension}";

        //    int suffix = 1;
        //    var blobClient = containerClient.GetBlobClient(blobName);
        //    while (await blobClient.ExistsAsync())
        //    {
        //        blobName = $"{originalName} ({suffix}){extension}";
        //        blobClient = containerClient.GetBlobClient(blobName);
        //        suffix++;
        //    }

        //    using (var stream = file.OpenReadStream())
        //    {
        //        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
        //    }

        //    return blobClient.Uri.ToString();
        //}
    }
}
