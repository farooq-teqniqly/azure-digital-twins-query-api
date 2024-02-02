using System.Net;
using FluentAssertions;
using Xunit;

namespace Teqniqly.AzureDigitalTwinsQueryApi.Tests
{
    public sealed class WelcomeIntegrationTests(CustomWebApplicationFactory fixture)
        : IClassFixture<CustomWebApplicationFactory>, IDisposable
    {
        private readonly ApiTestClient client = new(fixture.CreateClient());

        [Fact]
        public async Task Get_When_Successful_Returns_Welcome_Message()
        {
            var response = await client.GetAsync(Routes.Welcome);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var welcomeMessage = await ApiTestClient.ReadFromJsonAsync<string>(response);

            welcomeMessage.Should().Be("Azure Digital Twins Query API");
        }

        public void Dispose() => client.Dispose();
    }
}
