using FluentAssertions;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Salesforce;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.IntegrationTests
{
    public class SalesforceClientTests : TestApplicationFixture
    {
        private readonly ISalesforceRestClient _client;
        public SalesforceClientTests(EnterpriseBrokerFactory factory) : base(factory)
        {
            _client = Resolve<ISalesforceRestClient>();
        }


        [Fact]
        public async Task CreateOrder_AlredyExistingOrder_RetrunsBadRequest()
        {
            var result = await _client.GetOrderProducts("80163000002nGzAAAU", CancellationToken.None);
            result.Should().NotBeNullOrEmpty();
        }

    }
}