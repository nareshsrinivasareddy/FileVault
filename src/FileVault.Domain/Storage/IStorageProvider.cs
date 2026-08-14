namespace FileVault.Domain.Storage;

using FileVault.Domain.Enums;

public interface IStorageProvider
{
    StorageProviderType ProviderType { get; }

    Task<StorageUploadResult> UploadAsync(
        string containerName,
        string blobName,
        Stream fileStream,
        string contentType,
        CancellationToken cancellationToken);

    Task<Stream> DownloadAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken);
}
