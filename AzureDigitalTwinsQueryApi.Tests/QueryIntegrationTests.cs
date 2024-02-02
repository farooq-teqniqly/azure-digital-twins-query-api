using FluentAssertions;
using System.Net;
using Xunit;

namespace Teqniqly.AzureDigitalTwinsQueryApi.Tests
{
    public sealed class QueryIntegrationTests(CustomWebApplicationFactory fixture)
        : ApiIntegrationTests(fixture)
    {
        [Fact]
        public async Task Post_When_Successful_Returns_Expected_Results()
        {
            var requestBody = new QueryRequest { Query = "SELECT * FROM digitaltwins" };
            var response = await Client.PostAsync(Routes.Query, requestBody);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await ApiTestClient.ReadFromJsonAsync<QueryResult>(response);

            result.Twins.Count.Should().Be(4);
            result.QueryCharge.Should().Be(3.18F);
        }
    }
}
