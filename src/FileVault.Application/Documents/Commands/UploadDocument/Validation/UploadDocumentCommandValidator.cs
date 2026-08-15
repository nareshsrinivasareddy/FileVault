namespace FileVault.Application.Documents.Commands.UploadDocument.Validation;

using FluentValidation;

public sealed class UploadDocumentCommandValidator : AbstractValidator<UploadDocumentCommand>
{
    private const long MaximumFileSizeInBytes = 10 * 1024 * 1024;

    public UploadDocumentCommandValidator()
    {
        RuleFor(command => command.FileStream)
            .NotNull()
            .Must(stream => stream?.Length > 0)
            .WithMessage("A non-empty file is required.");

        RuleFor(command => command.FileName)
            .NotEmpty()
            .Must(fileName => string.Equals(Path.GetExtension(fileName), ".pdf", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Only PDF files are allowed.");

        RuleFor(command => command.ContentType)
            .Equal("application/pdf")
            .WithMessage("The file content type must be application/pdf.");

        RuleFor(command => command.SizeInBytes)
            .GreaterThan(0)
            .LessThanOrEqualTo(MaximumFileSizeInBytes)
            .WithMessage("The file size must be between 1 byte and 10 MB.");
    }
}