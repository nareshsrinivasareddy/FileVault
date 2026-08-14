namespace FileVault.Domain.Storage;

using FileVault.Domain.Enums;

public interface IStorageProviderFactory
{
    IStorageProvider GetProvider(StorageProviderType? providerType = null);
}
