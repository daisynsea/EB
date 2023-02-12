using FluentAssertions;
using Kymeta.Cloud.Services.EnterpriseBroker.HttpClients;
using Kymeta.Cloud.Services.EnterpriseBroker.IntegrationTests;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.Orders;

namespace IntegrationTests
{
    public class OracleClientTests
    {
        private readonly IOracleClient _client;
        public OracleClientTests()
        {
            _client = TestApplication.GetRequiredService<IOracleClient>();
        }

        [Fact]
        public async Task CreateOrder_InvalidOrder_RetrunsCreatedWithPayloadIssues()
        {
            Random random = new Random();

            var order = new CreateOrderBuilder()
                 .WithSourceTransactionNumber(random.Next(1000000, 9999999).ToString())
                 .WithSourceTransactionId(random.Next(1000,9999).ToString())
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
            result.Data.Should().NotBeNull();
            result.Data.ToString().Should().Contain("\"MessageText\" : \"The application didn't submit the sales order because it failed validation.\"");
        }

        [Fact]
        public async Task CreateOrder_ValidOrder_Retrunsuccess()
        {
            Random random = new Random();

            var order = new CreateOrderBuilder()
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
            result.Data.Should().NotBeNull();
            result.Data.ToString().Should().Contain("\"MessageText\" : \"The application successfully imported the sales order.\"");
        }

        [Fact]
        public async Task CreateOrder_AlredyExistingOrder_RetrunsBadRequest()
        {
            var order = new CreateOrderBuilder()
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
            result.Data.Should().Be("The request failed because a sales order with transaction 0047355 from source system OPS already exists.");
        }

    }
}