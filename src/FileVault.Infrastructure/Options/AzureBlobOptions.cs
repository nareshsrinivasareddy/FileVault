namespace FileVault.Infrastructure.Options;

public sealed class AzureBlobOptions
{
    public const string SectionName = "AzureBlob";

    public string ContainerName { get; set; } = "documents";
}
