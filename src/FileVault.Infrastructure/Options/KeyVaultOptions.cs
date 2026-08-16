namespace FileVault.Infrastructure.Options;

public sealed class KeyVaultOptions
{
    public const string SectionName = "KeyVault";

    public string Uri { get; set; } = string.Empty;
    public KeyVaultSecretOptions Secrets { get; set; } = new();
    public KeyVaultKeyOptions Keys { get; set; } = new();
    public KeyVaultCertificateOptions Certificates { get; set; } = new();
}

public sealed class KeyVaultSecretOptions
{
    public string BlobStorageConnectionString { get; set; } = string.Empty;
}

public sealed class KeyVaultKeyOptions
{
    public string FileEncryption { get; set; } = string.Empty;
}

public sealed class KeyVaultCertificateOptions
{
    public string ApiCertificate { get; set; } = string.Empty;
}