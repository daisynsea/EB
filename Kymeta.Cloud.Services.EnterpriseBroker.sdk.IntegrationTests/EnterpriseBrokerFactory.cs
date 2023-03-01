using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.IntegrationTests;

public class EnterpriseBrokerFactory : WebApplicationFactory<Program>
{
    public HttpClient Client { get; }
    public EnterpriseBrokerFactory()
    {
        //Client = CreateClient(); //Setup correctly base and needed headers
        Client = TestHostFactory.Host.CreateClient();
    }
}


internal static class TestHostFactory
{
    internal static WebApplicationFactory<Program> Host = new WebApplicationFactory<Program>();
}