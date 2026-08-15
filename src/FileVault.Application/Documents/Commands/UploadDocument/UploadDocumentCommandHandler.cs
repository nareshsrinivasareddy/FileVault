namespace FileVault.Application.Documents.Commands.UploadDocument;

using FileVault.Domain.Entities;
using FileVault.Domain.Storage;

public sealed class UploadDocumentCommandHandler
{
    private readonly IStorageProviderFactory _storageProviderFactory;

    public UploadDocumentCommandHandler(IStorageProviderFactory storageProviderFactory)
    {
        _storageProviderFactory = storageProviderFactory;
    }

    public async Task<UploadDocumentResult> UploadAsync(UploadDocumentCommand request, CancellationToken cancellationToken)
    {
        var provider = _storageProviderFactory.GetProvider(request.PreferredProvider);

        var blobName = $"{Guid.NewGuid():N}_{Path.GetFileName(request.FileName)}";

        request.FileStream.Position = 0;

        var uploadResult = await provider.UploadAsync(string.Empty, blobName, request.FileStream, request.ContentType, cancellationToken);

        var document = DocumentFile.Create(request.FileName, request.ContentType, uploadResult.SizeInBytes, uploadResult.ContainerName, uploadResult.BlobName, provider.ProviderType, uploadResult.DownloadUrl);

        return new UploadDocumentResult(document.Id, document.FileName, document.ContentType, document.SizeInBytes, document.ContainerName, document.BlobName, document.ProviderType, document.DownloadUrl, document.CreatedUtc);
    }
}
