namespace FileVault.Api.Endpoints;

using FileVault.Application.Documents.Commands.UploadDocument;
using FluentValidation;

public static class DocumentsEndpoint
{
    public static void MapDocumentsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/documents");

        group.MapPost("/upload", async (IFormFile file, IValidator<UploadDocumentCommand> validator, UploadDocumentCommandHandler handler, CancellationToken cancellationToken) =>
        {
            await using var stream = file.OpenReadStream();
            var command = new UploadDocumentCommand(file.FileName, file.ContentType, file.Length, stream, Domain.Enums.StorageProviderType.AzureBlob);

            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var result = await handler.UploadAsync(command, cancellationToken);
            return Results.Ok(result);
        })
        .DisableAntiforgery()
        .Accepts<IFormFile>("multipart/form-data")
        .WithName("UploadDocument")
        .WithSummary("Upload a PDF document to Azure Blob Storage.");
    }
}
