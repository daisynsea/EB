using Microsoft.AspNetCore.Mvc.Testing;

namespace Kymeta.Cloud.Services.EnterpriseBroker.IntegrationTests;

public class EnterpriseBrokerFactory : WebApplicationFactory<Program>
{
    public HttpClient Client { get; }
    public EnterpriseBrokerFactory()
    {
        Client = CreateClient();
    }
}