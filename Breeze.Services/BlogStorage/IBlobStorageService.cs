using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace Breeze.Services.BlogStorage;
public interface IBlobStorageService
{
    Task<string> UploadFileAsync(IFormFile file, string blobContainerName);
    Task<BlobDownloadInfo> DownloadFileAsync(string blobName, string blobContainerName);
    Task DeleteFileAsync(string blobName, string blobContainerName);
    Task<bool> DoesBlobExistAsync(string blobName, string blobContainerName);
}
