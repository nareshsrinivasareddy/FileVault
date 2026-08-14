namespace FileVault.Api.Endpoints;

using FileVault.Application.Documents.Commands.UploadDocument;
using MediatR;

public static class DocumentsEndpoint
{
    public static void MapDocumentsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/documents");

        group.MapPost("/upload", async (IFormFile file, ISender sender, CancellationToken cancellationToken) =>
        {
            if (file is null || file.Length == 0)
            {
                return Results.BadRequest("File is required.");
            }

            await using var stream = file.OpenReadStream();
            var command = new UploadDocumentCommand(
                file.FileName,
                file.ContentType,
                file.Length,
                stream,
                FileVault.Domain.Enums.StorageProviderType.AzureBlob);

            var result = await sender.Send(command, cancellationToken);
            return Results.Ok(result);
        })
        .DisableAntiforgery()
        .Accepts<IFormFile>("multipart/form-data")
        .WithName("UploadDocument")
        .WithSummary("Upload a PDF document to Azure Blob Storage.");
    }
}
