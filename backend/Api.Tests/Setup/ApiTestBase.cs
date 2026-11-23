namespace Api.Tests.Setup;

public abstract class ApiTestBase : IAsyncLifetime
{
    protected PortfolioApiFactory Factory { get; }

    protected ApiTestBase(PortfolioApiFactory factory)
    {
        Factory = factory;
    }

    public Task InitializeAsync()
    {
        return Factory.LeegDataAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
