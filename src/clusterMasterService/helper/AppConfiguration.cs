using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;

namespace ClusterMaster3000.clusterMasterService.helper
{
    class AppConfiguration
    {
        public string? HetznerApiKey { get; }
        public string? EncryptionKey { get; }

        public AppConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            HetznerApiKey = configuration["HETZNERAPIKEY"];
            EncryptionKey = configuration["ENCRYPTIONKEY"];
        }

        public void AddConfigurationInJson(string key, string value)
        {
            var config = File.ReadAllText("appsettings.json");

            var jsonObj = JsonObject.Parse(config);
            jsonObj.AsObject().Add(key, value);

            File.WriteAllText("appsettings.json", jsonObj.ToString());
        }
    }
}
