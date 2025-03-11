using Microsoft.Extensions.Configuration;

namespace ClusterMaster3000.classes.helper
{
    class AppConfiguration
    {
        public string ApiKey { get; }
        public string Environment { get; }
        public string SshKey { get; }

        public AppConfiguration()
        {
            var settingsFile = "appsettings.json";
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(settingsFile, optional: false, reloadOnChange: true)
                .Build();

            ApiKey = configuration["provider:hetzner:apiKey"] ?? throw new ArgumentNullException("Apikey not found");
            Environment = configuration["generalSettings:environment"] ?? throw new ArgumentNullException("Environment not found");
            SshKey = configuration["provider:hetzner:sshKey"] ?? throw new ArgumentNullException("SSH Key not found");
        }
    }
}
