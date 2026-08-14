namespace FileVault.Infrastructure;

using FileVault.Domain.Storage;
using FileVault.Infrastructure.Options;
using FileVault.Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureBlobOptions>(configuration.GetSection(AzureBlobOptions.SectionName));

        services.AddSingleton<IStorageProvider, AzureBlobStorageProvider>();
        services.AddSingleton<IStorageProviderFactory, StorageProviderFactory>();

        return services;
    }
}
