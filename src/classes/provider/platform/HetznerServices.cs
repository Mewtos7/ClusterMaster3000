using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ClusterMaster3000.classes.helper;

namespace ClusterMaster3000.classes.provider.platform
{
    class HetznerServices
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly AppConfiguration config = new AppConfiguration();

        public HetznerServices()
        {
            var apiKey = config.HetznerApiKey;
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }

        public async Task<string> GetServers()
        {
            using HttpResponseMessage
                response = await httpClient.GetAsync("https://api.hetzner.cloud/v1/servers");

            var message = await response.Content.ReadAsStringAsync();
            return message;
        }

        //TODO: make things variable for the server creation, like store server_type in config n stuff
        //TODO: handle exceptions
        public async Task<string> CreateServer(string sshKey)
        {
            Random random = new Random();
            var servername = "eva" + random.Next(0, 1000) + "-" + random.Next(0, 1000);


            using StringContent jsonContent = new(
                JsonSerializer.Serialize(new
                {
                    image = "ubuntu-24.04",
                    name = servername,
                    server_type = "cx22",
                    ssh_keys = new[] { sshKey }
                }),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage
                response = await httpClient.PostAsync("https://api.hetzner.cloud/v1/servers", jsonContent);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            return jsonResponse;
        }



        public async Task DeleteServer(string id)
        {

            using HttpResponseMessage
                response = await httpClient.DeleteAsync($"https://api.hetzner.cloud/v1/servers/{id}");

            var message = await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DepositPublicSshKey(string publicKey)
        {
            var guid = Guid.NewGuid().ToString();

            using StringContent jsonContent = new(
                JsonSerializer.Serialize(new
                {
                    name = guid,
                    public_key = publicKey
                }),
                Encoding.UTF8,
                "application/json");


            using HttpResponseMessage
                response = await httpClient.PostAsync("https://api.hetzner.cloud/v1/ssh_keys", jsonContent);

            var message = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            return guid;
        }
    }
}
