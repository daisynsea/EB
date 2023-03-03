

using Microsoft.AspNetCore.Mvc.Testing;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.IntegrationTests;

public class EnterpriseBrokerFactory
{
    public HttpClient Client { get; }
    internal WebApplicationFactory<Program> Host { get; }
    public EnterpriseBrokerFactory()
    {
        Host = TestHostFactory.Host;
        Client = Host.CreateClient();
    }
}

internal static class TestHostFactory
{
    internal static readonly WebApplicationFactory<Program> Host = new();
}