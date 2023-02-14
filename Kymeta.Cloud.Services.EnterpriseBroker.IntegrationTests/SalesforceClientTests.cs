using FluentAssertions;
using Kymeta.Cloud.Services.EnterpriseBroker.HttpClients;
using Kymeta.Cloud.Services.EnterpriseBroker.IntegrationTests;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.Orders;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;

namespace IntegrationTests
{
    public class SalesforceClientTests: TestApplicationFixture
    {
        private readonly ISalesforceClient _client;
        public SalesforceClientTests(EnterpriseBrokerFactory factory) : base(factory)
        {
            _client = Resolve<ISalesforceClient>();
        }

       
        [Fact]
        public async Task CreateOrder_AlredyExistingOrder_RetrunsBadRequest()
        {
            var result = await _client.GetOrderProducts("0047355", CancellationToken.None);
            result.Should().NotBeEmpty();
        }

    }
}