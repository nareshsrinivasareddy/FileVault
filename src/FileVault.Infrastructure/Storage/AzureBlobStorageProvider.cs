namespace FileVault.Infrastructure.Storage;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FileVault.Domain.Enums;
using FileVault.Domain.Storage;
using FileVault.Infrastructure.Options;
using Microsoft.Extensions.Options;

public sealed class AzureBlobStorageProvider : IStorageProvider
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly AzureBlobOptions _options;

    public AzureBlobStorageProvider(IOptions<AzureBlobOptions> options)
    {
        _options = options.Value;
        _blobServiceClient = new BlobServiceClient(_options.ConnectionString);
    }

    public StorageProviderType ProviderType => StorageProviderType.AzureBlob;

    public async Task<StorageUploadResult> UploadAsync(string containerName, string blobName, Stream fileStream, string contentType, CancellationToken cancellationToken)
    {
        var resolvedContainerName = string.IsNullOrWhiteSpace(containerName) ? _options.ContainerName : containerName;
        var containerClient = _blobServiceClient.GetBlobContainerClient(resolvedContainerName);
        await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        var blobClient = containerClient.GetBlobClient(blobName);
        var headers = new BlobHttpHeaders { ContentType = contentType };

        fileStream.Position = 0;
        await blobClient.UploadAsync(fileStream, new BlobUploadOptions { HttpHeaders = headers }, cancellationToken);

        var url = blobClient.Uri.ToString();
        return new StorageUploadResult(resolvedContainerName, blobName, url, fileStream.Length);
    }

    public async Task<Stream> DownloadAsync(string containerName, string blobName, CancellationToken cancellationToken)
    {
        var blobClient = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        var response = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);
        var stream = new MemoryStream();
        await response.Value.Content.CopyToAsync(stream, cancellationToken);
        stream.Position = 0;
        return stream;
    }

    public async Task DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken)
    {
        var blobClient = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }
}
