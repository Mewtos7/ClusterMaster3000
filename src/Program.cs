using System;
using System.Buffers;
using System.Text.Json;
using ClusterManager3000.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;


namespace ClusterManager3000
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var p = new Program();
            await p.OrchestrateServers();
        }

        private async Task OrchestrateServers()
        {
            HetznerServices hetznerServices = new HetznerServices();
        }
    }
}
