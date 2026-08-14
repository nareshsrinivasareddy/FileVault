namespace FileVault.Domain.Storage;

public sealed record StorageUploadResult(
    string ContainerName,
    string BlobName,
    string DownloadUrl,
    long SizeInBytes);
