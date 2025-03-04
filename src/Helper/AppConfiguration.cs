using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ClusterManager3000.Helper
{
    class AppConfiguration
    {
        public string ApiKey { get; }
        public string Environment {  get; }
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
