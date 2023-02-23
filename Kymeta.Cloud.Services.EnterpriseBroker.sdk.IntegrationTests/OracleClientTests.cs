using FluentAssertions;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.IntegrationTests
{
    public class OracleClientTests : TestApplicationFixture
    {
        private readonly IOracleRestClient _client;
        private string OrderNumberExistsInOracle = "280120";
        private string OrderNumberNotInOracle = "999999";
        public OracleClientTests(EnterpriseBrokerFactory factory) : base(factory)
        {
            _client = Resolve<IOracleRestClient>();
        }

        [Fact]
        public async Task GetOrder_OrderExistsInOracle_ReturnsOrder()
        {
            var found = await _client.GetOrder(OrderNumberExistsInOracle, default);
            var payload = found.Payload;
            payload.Should().NotBeNull();
            payload.IsSuccessfulResponse().Should().BeTrue();
            payload.HeaderId.Should().Be(300000113543461);
            payload.OrderNumber.Should().Be("3472712");
            payload.SourceTransactionNumber.Should().Be("3472712");
            payload.SourceTransactionSystem.Should().Be("OPS");
            payload.SourceTransactionId.Should().Be("4937");
            payload.BusinessUnitId.Should().Be(300000001130195);
            payload.BusinessUnitName.Should().Be("Kymeta Corporation BU");
        }

        [Fact]
        public async Task GetOrder_OrderNotInOracle_ReturnsOrder()
        {
            OracleResponse<GetOrderResponse> found = await _client.GetOrder(OrderNumberNotInOracle, default);
            found.Payload.IsSuccessfulResponse().Should().BeFalse();
        }

        [Fact]
        public async Task CreateOrder_InvalidOrder_ReturnsCreatedWithPayloadIssues()
        {
            Random random = new Random();

            var order = new OracleCreateOrderBuilder()
                 .WithSourceTransactionNumber(random.Next(1000000, 9999999).ToString())
                 .WithSourceTransactionId(random.Next(1000, 9999).ToString())
                 .WithOrderKey("OPS:0047355")
                 .WithSourceTransactionSystem("OPS")
                 .WithBusinessUnitName("Kymeta Corporation BU")
                 .WithBuyingPartyNumber("423011")
                 .WithBuyingPartyContactNumber("423012")
                 .WithTransactionType("Standard Orders")
                 .WithRequestedShipDate("2023-10-19T20:49:12+00:00")
                 .WithPaymentTerms("Net 60")
                 .WithTransactionalCurrencyCode("USD")
                 .WithRequestingBusinessUnitName("Kymeta Corporation BU")
                 .WithFreezePriceFlag(false)
                 .WithFreezeShippingChargeFlag(false)
                 .WithFreezeTaxFlag(false)
                 .WithSubmittedFlag(true)
                 .WithSourceTransactionRevisionNumber("1")
                 .Build();

            var result = await _client.CreateOrder(order, CancellationToken.None);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            result.Content.Should().NotBeNull();
            result.Content.ToString().Should().Contain("\"MessageText\" : \"The application didn't submit the sales order because it failed validation.\"");
        }

        [Fact]
        public async Task CreateOrder_ValidOrder_Retrunsuccess()
        {
            Random random = new Random();

            var order = new OracleCreateOrderBuilder()
                 .WithSourceTransactionNumber(random.Next(1000000, 9999999).ToString())
                 .WithSourceTransactionId(random.Next(1000, 9999).ToString())
                 .WithOrderKey("OPS:0047355")
                 .WithSourceTransactionSystem("OPS")
                 .WithBusinessUnitName("Kymeta Corporation BU")
                 .WithBuyingPartyNumber("423011")
                 .WithBuyingPartyContactNumber("423012")
                 .WithTransactionType("Standard Orders")
                 .WithRequestedShipDate("2023-10-19T20:49:12+00:00")
                 .WithPaymentTerms("Net 60")
                 .WithTransactionalCurrencyCode("USD")
                 .WithRequestingBusinessUnitName("Kymeta Corporation BU")
                 .WithFreezePriceFlag(false)
                 .WithFreezeShippingChargeFlag(false)
                 .WithFreezeTaxFlag(false)
                 .WithSubmittedFlag(true)
                 .WithSourceTransactionRevisionNumber("1")
                 .WithLines(new List<OrderLines>
                 {
                    new OrderLines
                    {
                        SourceTransactionLineId = "1",
                        SourceTransactionLineNumber = "1",
                        SourceTransactionScheduleId = "1",
                        SourceScheduleNumber = "1",
                        TransactionCategoryCode = "ORDER",
                        TransactionLineType = "Buy",
                        ProductNumber = "U8911-11113-0",
                        OrderedQuantity = "80",
                        OrderedUOM = "EA"
                    }
                 })
                 .Build();

            var result = await _client.CreateOrder(order, CancellationToken.None);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            result.Content.Should().NotBeNull();
            result.Content.ToString().Should().Contain("\"MessageText\" : \"The application successfully imported the sales order.\"");
        }

        [Fact]
        public async Task CreateOrder_AlredyExistingOrder_RetrunsBadRequest()
        {
            var order = new OracleCreateOrderBuilder()
                .WithSourceTransactionNumber("0047355")
                .WithSourceTransactionId("0047355")
                .WithOrderKey("OPS:0047355")
                .WithSourceTransactionSystem("OPS")
                .WithBusinessUnitName("Kymeta Corporation BU")
                .WithBuyingPartyNumber("423011")
                .WithBuyingPartyContactNumber("423012")
                .WithTransactionType("Standard Orders")
                .WithRequestedShipDate("2023-10-19T20:49:12+00:00")
                .WithPaymentTerms("Net 60")
                .WithTransactionalCurrencyCode("USD")
                .WithRequestingBusinessUnitName("Kymeta Corporation BU")
                .WithFreezePriceFlag(false)
                .WithFreezeShippingChargeFlag(false)
                .WithFreezeTaxFlag(false)
                .WithSubmittedFlag(true)
                .WithSourceTransactionRevisionNumber("1")
                .WithBillToCustomer(new CustomerBill { AccountNumber = "77001" })
                .WithShipToCustomer(new CustomerShip { PartyNumber = "423011" })
                .WithLines(new List<OrderLines>
                {
                    new OrderLines
                    {
                        SourceTransactionLineId = "1",
                        SourceTransactionLineNumber = "1",
                        SourceTransactionScheduleId = "1",
                        SourceScheduleNumber = "1",
                        TransactionCategoryCode = "ORDER",
                        TransactionLineType = "Buy",
                        ProductNumber = "U8911-11113-0",
                        OrderedQuantity = "80",
                        OrderedUOM = "EA"
                    },
                    new OrderLines
                    {
                        SourceTransactionLineId = "2",
                        SourceTransactionLineNumber = "2",
                        SourceTransactionScheduleId = "1",
                        SourceScheduleNumber = "1",
                        TransactionCategoryCode = "ORDER",
                        TransactionLineType = "Buy",
                        ProductNumber = "U8911-11113-0",
                        OrderedQuantity = "1",
                        OrderedUOM = "EA"
                    }
                })
                .Build();


            var result = await _client.CreateOrder(order, CancellationToken.None);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            result.Message.Should().Be("Bad Request");
            result.Content.Should().Be("The request failed because a sales order with transaction 0047355 from source system OPS already exists.");
        }

    }
}