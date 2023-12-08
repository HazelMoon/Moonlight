using Moonlight.App.Database.Entities.Servers;

namespace Moonlight.App.Extensions;

public static class ServerNodeExtensions
{
    public static HttpClient CreateHttpClient(this ServerNode node)
    {
        var httpClient = new HttpClient();
        
        httpClient.DefaultRequestHeaders.Add("Authorization", node.Token);

        if(node.UseSsl)
            httpClient.BaseAddress = new Uri($"https://{node.Fqdn}:{node.HttpPort}/");
        else
            httpClient.BaseAddress = new Uri($"http://{node.Fqdn}:{node.HttpPort}/");

        return httpClient;
    }
}