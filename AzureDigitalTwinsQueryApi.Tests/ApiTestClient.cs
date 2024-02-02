using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Teqniqly.AzureDigitalTwinsQueryApi.Tests
{
    public class ApiTestClient(HttpClient httpClient)
    {
        private readonly HttpClient client = httpClient;

        public async Task<HttpResponseMessage> PostAsync(string endpoint, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await client.PostAsync(endpoint, content);
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint) => await client.GetAsync(endpoint);

        public static async Task<T> ReadFromJsonAsync<T>(HttpResponseMessage response)
            => await response.Content.ReadFromJsonAsync<T>()
               ?? throw new InvalidOperationException(
                   $"Could not deserialize the result into the requested type {typeof(T)}");

        public void Dispose() => client.Dispose();
    }
}
