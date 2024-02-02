using FluentAssertions;
using System.Net;
using Xunit;

namespace Teqniqly.AzureDigitalTwinsQueryApi.Tests
{
    public sealed class QueryIntegrationTests(CustomWebApplicationFactory fixture)
        : IClassFixture<CustomWebApplicationFactory>, IDisposable
    {
        private readonly ApiTestClient client = new(fixture.CreateClient());

        [Fact]
        public async Task Post_When_Successful_Returns_Created_Status_Code()
        {
            var requestBody = new QueryRequest { Query = "SELECT * FROM digitaltwins" };
            var response = await client.PostAsync("api/q", requestBody);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Post_When_Successful_Returns_Expected_Results()
        {
            var requestBody = new QueryRequest { Query = "SELECT * FROM digitaltwins" };
            var response = await client.PostAsync("api/q", requestBody);
            var result = await ApiTestClient.ReadFromJsonAsync<QueryResult>(response);

            result.Twins.Count.Should().Be(4);
            result.QueryCharge.Should().Be(3.18F);
        }

        public void Dispose() => client.Dispose();
    }
}
