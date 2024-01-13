using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace Breeze.Services.BlogStorage;
public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService(string connectionString)
    {
        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public async Task<string> UploadFileAsync(IFormFile file, string blobContainerName)
    {
        string blobName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

        BlobContainerClient containerClient = GetContainerClient(blobContainerName);

        await containerClient.CreateIfNotExistsAsync();

        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: true);

        return blobClient.Uri.ToString();
    }

    public async Task<BlobDownloadInfo> DownloadFileAsync(string blobName, string blobContainerName)
    {
        BlobContainerClient containerClient = GetContainerClient(blobContainerName);
        BlobClient blobClient = containerClient.GetBlobClient(blobName);
        BlobDownloadInfo download = await blobClient.DownloadAsync();
        return download;
    }

    public async Task DeleteFileAsync(string blobName, string blobContainerName)
    {
        BlobContainerClient containerClient = GetContainerClient(blobContainerName);
        BlobClient blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();
    }

    public async Task<bool> DoesBlobExistAsync(string blobName, string blobContainerName)
    {
        BlobContainerClient containerClient = GetContainerClient(blobContainerName);
        BlobClient blobClient = containerClient.GetBlobClient(blobName);
        return await blobClient.ExistsAsync();
    }

    private BlobContainerClient GetContainerClient(string blobContainerName)
    {
        return _blobServiceClient.GetBlobContainerClient(blobContainerName);
    }
}
