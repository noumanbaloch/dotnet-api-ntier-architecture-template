namespace Breeze.Services.SecretManager
{
    public interface ISecretManagerService
    {
        Task<string> GetSecretAsync(string secretName);
    }
}