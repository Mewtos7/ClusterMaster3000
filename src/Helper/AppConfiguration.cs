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
        
        public AppConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            ApiKey = configuration["Provider:Hetzner:APIKEY"] ?? throw new ArgumentNullException("APIKEY not found");
        }
    }
}
