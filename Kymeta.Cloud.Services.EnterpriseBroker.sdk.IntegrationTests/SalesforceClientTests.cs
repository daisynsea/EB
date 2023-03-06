using FluentAssertions;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;

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
            var orderWith10ProductsId = "80163000002nGzAAAU";
            var result = await _salesforceRestClient.GetOrderProducts( orderWith10ProductsId, CancellationToken.None);
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(10);
        }

        [Fact]
        public async Task SyncFromOracle_UpdatedOrder_RetrunsSuccess()
        {
            OracleResponse<GetOrderResponse> found = await _oracleRestClient.GetOrder(OrderUpdateExistsInOracle, default);
            Item foundPayload = found.Payload.FindLatestRevision();
            var salesForceOracleSync = new OracleSalesforceSyncRequest()
            {
                CompositeRequest = new List<CompositeRequest>()
                {
                    new CompositeRequest()
                    {
                        Url = $"/services/data/v57.0/sobjects/order/{OrderUpdateExistsInOracle}",
                        ReferenceId = "referenceId",
                        Body = new OracleOrderStatusSync()
                        {
                            Oracle_SO__c = foundPayload.OrderNumber,
                            Oracle_Status__c = IntegrationConstants.Activated,
                            Oracle_Sync_Status__c = IntegrationConstants.Successful,
                            NEO_Oracle_Integration_Error__c = IntegrationConstants.Clear,
                            NEO_Oracle_Integration_Status__c = IntegrationConstants.Success,
                            NEO_Oracle_Sales_Order_Id__c = foundPayload.HeaderId.ToString()
                        }
                    },
                }, 
            };
            var result = await _salesforceRestClient.SyncFromOracle(salesForceOracleSync, default);
            result.Should().NotBeNull();
        }

    }
}