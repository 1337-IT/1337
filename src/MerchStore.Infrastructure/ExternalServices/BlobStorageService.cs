using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;


namespace MerchStore.Infrastructure.ExternalServices;

public class BlobStorageService
{
    private readonly BlobContainerClient _container;

    public BlobStorageService(IOptions<BlobStorageSettings> options)
    {
        var config = options.Value;
        var blobServiceClient = new BlobServiceClient(config.ConnectionString);
        _container = blobServiceClient.GetBlobContainerClient(config.ContainerName);
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var blobClient = _container.GetBlobClient(fileName);

        await using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: true);

        return blobClient.Uri.ToString();
    }
}

public class BlobStorageSettings
{
    public string ConnectionString { get; set; } = "";
    public string ContainerName { get; set; } = "";
}
