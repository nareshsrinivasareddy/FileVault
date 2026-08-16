namespace FileVault.Domain.Security;

public interface ISecretProvider
{
    Task<string> GetSecretAsync(string name, CancellationToken cancellationToken);
}