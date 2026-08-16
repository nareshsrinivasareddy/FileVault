namespace FileVault.Infrastructure;

using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FileVault.Domain.Security;
using FileVault.Domain.Storage;
using FileVault.Infrastructure.Options;
using FileVault.Infrastructure.Secrets;
using FileVault.Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureBlobOptions>(configuration.GetSection(AzureBlobOptions.SectionName));
        services.Configure<KeyVaultOptions>(configuration.GetSection(KeyVaultOptions.SectionName));

        var keyVaultOptions = configuration.GetSection(KeyVaultOptions.SectionName).Get<KeyVaultOptions>() ?? throw new InvalidOperationException("Key Vault configuration is missing.");

        if (string.IsNullOrWhiteSpace(keyVaultOptions.Uri))
        {
            throw new InvalidOperationException("Key Vault URI is not configured.");
        }

        //removed credential and secrets need to store in the environment variable and use the default credentials
        var credential = new ClientSecretCredential("", "", "");
        services.AddSingleton<TokenCredential>(credential);
        services.AddSingleton(new SecretClient(new Uri(keyVaultOptions.Uri), new DefaultAzureCredential()));

        services.AddSingleton<ISecretProvider, AzureKeyVaultSecretProvider>();
        services.AddSingleton<IStorageProvider, AzureBlobStorageProvider>();
        services.AddSingleton<IStorageProviderFactory, StorageProviderFactory>();

        return services;
    }
}