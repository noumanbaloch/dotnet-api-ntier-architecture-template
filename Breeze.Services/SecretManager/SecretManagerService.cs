using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Breeze.Services.SecretManager
{
    public class SecretManagerService : ISecretManagerService
    {
        private readonly SecretClient _secretClient;

        public SecretManagerService(string keyVaultUrl)
        {
            var credential = new DefaultAzureCredential();
            _secretClient = new SecretClient(new Uri(keyVaultUrl), credential);
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);
            return secret.Value;
        }
    }
}
