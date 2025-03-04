using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ClusterManager3000.Helper
{
    internal class HetznerServices
    {
        private readonly HttpClient httpClient = new HttpClient();

        public HetznerServices()
        {
            var config = new AppConfiguration();
            string apiKey = config.ApiKey;
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }

        public async Task<string> GetServers()
        {
            using HttpResponseMessage
                response = await httpClient.GetAsync("https://api.hetzner.cloud/v1/servers");

            var message = await response.Content.ReadAsStringAsync();
            return message;
        }

        //TODO: Possible null-reference
        public async Task<string> GetServerByname(string serverName)
        {
            var servers = GetServers().Result;
            JObject jsonObject = JObject.Parse(servers);
            var item = jsonObject["servers"].FirstOrDefault(i => i["name"].ToString() == serverName);
            var serverId = item["id"].ToString();
            return serverId;
        }

        //TODO: make things variable for the server creation
        public async Task<string> CreateServer()
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(new
                {
                    image = "ubuntu-24.04",
                    name = "test",
                    server_type = "cx22"
                }),
                System.Text.Encoding.UTF8,
                "application/json");


            using HttpResponseMessage
                response = await httpClient.PostAsync("https://api.hetzner.cloud/v1/servers", jsonContent);

            var message = await response.Content.ReadAsStringAsync();
            return message;
        }

        public async Task DeleteServer(string id)
        {

            using HttpResponseMessage
                response = await httpClient.DeleteAsync($"https://api.hetzner.cloud/v1/servers/{id}");

            var message = await response.Content.ReadAsStringAsync();
        }

    }
}
