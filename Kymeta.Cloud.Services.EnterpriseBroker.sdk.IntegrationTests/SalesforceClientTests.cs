using FluentAssertions;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.IntegrationTests
{
    public class SalesforceClientTests : TestApplicationFixture
    {
        private readonly ISalesforceRestApi _client;
        public SalesforceClientTests(EnterpriseBrokerFactory factory) : base(factory)
        {
            _client = Resolve<ISalesforceClient2>().Rest;
        }


        [Fact]
        public async Task CreateOrder_AlredyExistingOrder_RetrunsBadRequest()
        {
            var result = await _client.GetOrderProducts("80163000002nGzAAAU", CancellationToken.None);
            result.Should().NotBeNullOrEmpty();
        }

    }
}