using Microsoft.Extensions.Configuration;

namespace ClusterMaster3000.classes.helper
{
    class AppConfiguration
    {
        public string HetznerApiKey { get; }

        public AppConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            HetznerApiKey = configuration["HETZNERAPIKEY"] ?? throw new KeyNotFoundException("HetznerApiKey not found");
        }
    }
}
