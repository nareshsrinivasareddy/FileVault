namespace FileVault.Infrastructure.Storage;

using FileVault.Domain.Enums;
using FileVault.Domain.Storage;

public sealed class StorageProviderFactory : IStorageProviderFactory
{
    private readonly IReadOnlyDictionary<StorageProviderType, IStorageProvider> _providers;

    public StorageProviderFactory(IEnumerable<IStorageProvider> providers)
    {
        _providers = providers.ToDictionary(p => p.ProviderType);
    }

    public IStorageProvider GetProvider(StorageProviderType? providerType = null)
    {
        var resolvedType = providerType ?? StorageProviderType.AzureBlob;
        if (!_providers.TryGetValue(resolvedType, out var provider))
        {
            throw new InvalidOperationException($"Storage provider '{resolvedType}' is not registered.");
        }

        return provider;
    }
}
