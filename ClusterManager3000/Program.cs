using System;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;


namespace ClusterManager3000 {
    public class Program
    {
        private string apiKey = Environment.GetEnvironmentVariable("APIKEY") ?? throw new ArgumentNullException("APIKEY not found");

        static async Task Main(string[] args)
        {
            var p = new Program();
            await p.OrchestrateServers();
        }

        private async Task OrchestrateServers()
        {
            var mode = "delete"; //TODO: For testing

            HttpClient sharedClient = new();

            if (mode == "deploy")
                await deployServer(sharedClient);

            if (mode == "delete")
                await deleteServer(sharedClient, id: "");
        }

        public async Task getServers(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            using HttpResponseMessage
                response = await httpClient.GetAsync("https://api.hetzner.cloud/v1/servers");

            var message = await response.Content.ReadAsStringAsync();
            Console.WriteLine(message);
        }

        public async Task deployServer(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

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
            Console.WriteLine(message);
        }

        public async Task deleteServer(HttpClient httpClient, string id)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            using HttpResponseMessage
                response = await httpClient.DeleteAsync($"https://api.hetzner.cloud/v1/servers/{id}");

            var message = await response.Content.ReadAsStringAsync();
            Console.WriteLine(message);
        }
    }
}
