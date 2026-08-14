namespace FileVault.Domain.Entities;

using FileVault.Domain.Enums;

public sealed class DocumentFile
{
    public Guid Id { get; private set; }
    public string FileName { get; private set; } = string.Empty;
    public string ContentType { get; private set; } = string.Empty;
    public long SizeInBytes { get; private set; }
    public string ContainerName { get; private set; } = string.Empty;
    public string BlobName { get; private set; } = string.Empty;
    public StorageProviderType ProviderType { get; private set; }
    public string DownloadUrl { get; private set; } = string.Empty;
    public DateTimeOffset CreatedUtc { get; private set; }

    private DocumentFile() { }

    public static DocumentFile Create(
        string fileName,
        string contentType,
        long sizeInBytes,
        string containerName,
        string blobName,
        StorageProviderType providerType,
        string downloadUrl)
    {
        return new DocumentFile
        {
            Id = Guid.NewGuid(),
            FileName = fileName,
            ContentType = contentType,
            SizeInBytes = sizeInBytes,
            ContainerName = containerName,
            BlobName = blobName,
            ProviderType = providerType,
            DownloadUrl = downloadUrl,
            CreatedUtc = DateTimeOffset.UtcNow
        };
    }
}
