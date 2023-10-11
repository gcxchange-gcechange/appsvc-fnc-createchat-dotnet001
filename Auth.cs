using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System;

namespace appsvc_fnc_createchat_dotnet001
{
    internal class Auth
    {

        public static GraphServiceClient graphAuth(string authorizationHeader, ILogger log)
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables().Build();

            string keyVaultUrl = config["keyVaultUrl"];
            string secretName = config["secretName"];
            string clientId = config["clientId"];
            string tenantId = config["tenantId"];

            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                 }
            };

            var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential(), options);
            KeyVaultSecret secret = client.GetSecret(secretName);
            string clientSecret = secret.Value;

            OnBehalfOfCredential cred = new(tenantId, clientId, clientSecret, authorizationHeader);
            var graphClient = new GraphServiceClient(cred);

            return graphClient;
        }
    }
}