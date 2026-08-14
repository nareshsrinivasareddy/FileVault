namespace FileVault.Application.Documents.Commands.UploadDocument;

using FileVault.Domain.Enums;
using MediatR;

public sealed record UploadDocumentCommand(
    string FileName,
    string ContentType,
    long SizeInBytes,
    Stream FileStream,
    StorageProviderType? PreferredProvider = null) : IRequest<UploadDocumentResult>;
