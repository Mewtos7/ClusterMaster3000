
using System.Net.Http.Headers;

var mode = "deploy";
var apiKey = "9X4q746l3KnPrg33a0UK9TDT7DoIKPykwTA2qn9coO6PPI0IGgrE4IWmFoV67cUT";



HttpClient sharedClient = new()
{
    BaseAddress = new Uri("https://api.hetzner.cloud/v1/")
};



if (mode == "deploy") await deployServer(sharedClient);
if (mode == "destroy") destroyServer(sharedClient);



async Task deployServer(HttpClient httpClient) {
    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    using HttpResponseMessage 
        response = await httpClient.GetAsync("servers");
    
    var message = await response.Content.ReadAsStringAsync();
    Console.WriteLine(message);
}


void destroyServer(HttpClient httpClient)
{

}