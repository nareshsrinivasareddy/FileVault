namespace FileVault.Application.Documents.Commands.UploadDocument;

using FileVault.Domain.Enums;

public sealed record UploadDocumentResult(Guid Id, string FileName, string ContentType, long SizeInBytes, string ContainerName, string BlobName, StorageProviderType ProviderType, string DownloadUrl, DateTimeOffset CreatedUtc);
