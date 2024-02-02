using Xunit;

namespace Teqniqly.AzureDigitalTwinsQueryApi.Tests;

public abstract class ApiIntegrationTests(CustomWebApplicationFactory fixture)
    : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
    private bool disposed;
    protected ApiTestClient Client { get; } = new(fixture.CreateClient());

    protected virtual void Dispose(bool disposing)
    {
        if (disposed) return;

        if (disposing)
        {
            Client?.Dispose();
        }

        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}