using System.Net;
using FluentAssertions;
using Xunit;

namespace Teqniqly.AzureDigitalTwinsQueryApi.Tests
{
    public sealed class WelcomeIntegrationTests(CustomWebApplicationFactory fixture) : ApiIntegrationTests(fixture)
    {
        [Fact]
        public async Task Get_When_Successful_Returns_Welcome_Message()
        {
            var response = await Client.GetAsync(Routes.Welcome);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var welcomeMessage = await ApiTestClient.ReadFromJsonAsync<string>(response);

            welcomeMessage.Should().Be("Azure Digital Twins Query API");
        }
    }
}
