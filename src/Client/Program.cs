using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var client = new HttpClient();
            var discoveryDocumentResponse = await client.GetDiscoveryDocumentAsync("https://localhost:5001");

            if (discoveryDocumentResponse.IsError)
            {
                Console.WriteLine(discoveryDocumentResponse.Error);
                return;
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = discoveryDocumentResponse.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "test"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            
            Console.WriteLine("IdentityServer response:");
            Console.WriteLine(tokenResponse.Json);
            
            using var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("https://localhost:6001/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("API response:");
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}
