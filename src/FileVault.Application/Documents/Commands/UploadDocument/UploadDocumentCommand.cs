namespace FileVault.Application.Documents.Commands.UploadDocument;

using FileVault.Domain.Enums;

public sealed record UploadDocumentCommand(string FileName, string ContentType, long SizeInBytes, Stream FileStream, StorageProviderType? PreferredProvider = null);
