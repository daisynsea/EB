using FluentAssertions;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using StackExchange.Redis;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.IntegrationTests
{
    public class SalesforceClientTests : TestApplicationFixture
    {
        private readonly ISalesforceRestClient _salesforceRestClient;
        private readonly IOracleRestClient _oracleRestClient;
        private string OrderUpdateExistsInOracle = "114739004";
        public SalesforceClientTests(EnterpriseBrokerFactory factory) : base(factory)
        {
            _oracleRestClient = Resolve<IOracleRestClient>();
            _salesforceRestClient = Resolve<ISalesforceRestClient>();
        }


        [Fact]
        public async Task GetOrderProducts_ForExistingOrder_RetrunsAllProducts()
        {
            var orderWith4ProductsId = "80163000002nGUMAA2";
            var result = await _salesforceRestClient.GetOrderProducts( orderWith4ProductsId, CancellationToken.None);
            result.Should().NotBeNull();
            result.TotalSize.Should().Be(4);

            result.Records.Should().NotContain( x=>x.OrderId != orderWith4ProductsId);
            result.Records.Should().NotContain(x => x.NEO_Sync_to_Oracle__c  == false);
        }


        [Fact]
        public async Task GetOrderProducts_ForNonExistingOrder_RetrunsError()
        {
            var noOrder = "nope";
            Func<Task> act = async () => { await _salesforceRestClient.GetOrderProducts(noOrder, CancellationToken.None); };
            await act.Should().ThrowAsync<HttpRequestException>();
        }


        [Fact]
        public async Task SyncFromOracle_UpdatedOrder_RetrunsSuccess()
        {
            OracleResponse<GetOrderResponse> found = await _oracleRestClient.GetOrder(OrderUpdateExistsInOracle, default);
            Item foundPayload = found.Payload.FindLatestRevision();
            var salesForceOracleSync = OracleSalesforceSyncRequestBuilder.CreateRequest()
                .WithSuccessfulOrder("80163000002n8hSAAQ", foundPayload.OrderNumber, foundPayload.HeaderId)
                .Build();
            
            var result = await _salesforceRestClient.SyncFromOracle(salesForceOracleSync, default);
            result.Should().Be(System.Net.HttpStatusCode.OK);
        }

    }
}