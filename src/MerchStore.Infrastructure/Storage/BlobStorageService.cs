using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System.IO;


namespace MerchStore.Infrastructure.Storage;

public class BlobStorageService
{
    private readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=merchstoreblobstorage;AccountKey=7MUoHLJFvPwyKwKAoOs26ATDLWaru09z/ItgkIa7YzS0vbqOhGUdZRg+lEwHVm5Su4jZb+4hk+yv+AStEgjcYw==;EndpointSuffix=core.windows.net";
    private readonly string _containerName = "hats";

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var blobServiceClient = new BlobServiceClient(_connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var blobClient = containerClient.GetBlobClient(Guid.NewGuid() + Path.GetExtension(file.FileName));

        using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });

        return blobClient.Uri.ToString();
    }
}
