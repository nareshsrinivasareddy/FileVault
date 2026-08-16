namespace FileVault.Infrastructure.Storage;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FileVault.Domain.Enums;
using FileVault.Domain.Security;
using FileVault.Domain.Storage;
using FileVault.Infrastructure.Options;
using Microsoft.Extensions.Options;

public sealed class AzureBlobStorageProvider : IStorageProvider
{
    private readonly ISecretProvider _secretProvider;
    private readonly AzureBlobOptions _options;
    private readonly KeyVaultOptions _keyVaultOptions;
    private readonly Lazy<Task<BlobServiceClient>> _blobServiceClient;

    public AzureBlobStorageProvider(IOptions<AzureBlobOptions> options, IOptions<KeyVaultOptions> keyVaultOptions, ISecretProvider secretProvider)
    {
        _options = options.Value;
        _keyVaultOptions = keyVaultOptions.Value;
        _secretProvider = secretProvider;
        _blobServiceClient = new Lazy<Task<BlobServiceClient>>(CreateBlobServiceClientAsync);
    }

    public StorageProviderType ProviderType => StorageProviderType.AzureBlob;

    public async Task<StorageUploadResult> UploadAsync(string containerName, string blobName, Stream fileStream, string contentType, CancellationToken cancellationToken)
    {
        var blobServiceClient = await _blobServiceClient.Value;
        var resolvedContainerName = string.IsNullOrWhiteSpace(containerName) ? _options.ContainerName : containerName;
        var containerClient = blobServiceClient.GetBlobContainerClient(resolvedContainerName);

        var blobClient = containerClient.GetBlobClient(blobName);
        var headers = new BlobHttpHeaders { ContentType = contentType };

        fileStream.Position = 0;
        await blobClient.UploadAsync(fileStream, new BlobUploadOptions { HttpHeaders = headers });

        var url = blobClient.Uri.ToString();
        return new StorageUploadResult(resolvedContainerName, blobName, url, fileStream.Length);
    }

    public async Task<Stream> DownloadAsync(string containerName, string blobName, CancellationToken cancellationToken)
    {
        var blobServiceClient = await _blobServiceClient.Value;
        var blobClient = blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        var response = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);
        var stream = new MemoryStream();
        await response.Value.Content.CopyToAsync(stream, cancellationToken);
        stream.Position = 0;
        return stream;
    }

    public async Task DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken)
    {
        var blobServiceClient = await _blobServiceClient.Value;
        var blobClient = blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    private async Task<BlobServiceClient> CreateBlobServiceClientAsync()
    {
        if (string.IsNullOrWhiteSpace(_keyVaultOptions.Secrets.BlobStorageConnectionString))
        {
            throw new InvalidOperationException("Blob Storage connection string secret name is not configured.");
        }

        var connectionString = await _secretProvider.GetSecretAsync(_keyVaultOptions.Secrets.BlobStorageConnectionString, CancellationToken.None);
        return new BlobServiceClient(connectionString);
    }
}
