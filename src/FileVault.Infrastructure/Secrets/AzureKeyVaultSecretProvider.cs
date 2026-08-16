namespace FileVault.Infrastructure.Secrets;

using Azure.Security.KeyVault.Secrets;
using FileVault.Domain.Security;

public sealed class AzureKeyVaultSecretProvider : ISecretProvider
{
    private readonly SecretClient _secretClient;

    public AzureKeyVaultSecretProvider(SecretClient secretClient)
    {
        _secretClient = secretClient;
    }

    public async Task<string> GetSecretAsync(string name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Secret name is required.", nameof(name));
        }

        var response = await _secretClient.GetSecretAsync(name);

        return response.Value.Value;
    }
}