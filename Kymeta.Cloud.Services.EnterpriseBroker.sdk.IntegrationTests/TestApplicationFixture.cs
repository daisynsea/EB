using Microsoft.Extensions.DependencyInjection;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.IntegrationTests;

[CollectionDefinition("Integration")]
public class TestApplicationFixture : IClassFixture<EnterpriseBrokerFactory>
{
    private readonly EnterpriseBrokerFactory _factory;

    protected TestApplicationFixture(EnterpriseBrokerFactory factory)
    {
        _factory = factory;
        Client = factory.Client;
    }

    public HttpClient Client { get; }

    protected T Resolve<T>() where T : notnull
    {
        return _factory.Host.Services.GetRequiredService<T>();
    }
}