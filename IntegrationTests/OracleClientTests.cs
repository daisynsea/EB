using FluentAssertions;
using Kymeta.Cloud.Services.EnterpriseBroker.HttpClients;
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
        public async Task CreateOrder_InvalidOrder_RetrunsBadRequest()
        {
            CreateOrder order = new CreateOrder
            {
                SourceTransactionNumber = "123456",
                SourceTransactionId = "ABC123",
                OrderKey = "ORDER-123",
                SourceTransactionSystem = "System A",
                BusinessUnitName = "Business Unit A",
                BuyingPartyNumber = "789",
                BuyingPartyContactNumber = "456",
                TransactionType = "Type A",
                RequestedShipDate = "2023-02-15",
                PaymentTerms = "NET30",
                TransactionalCurrencyCode = "USD",
                RequestingBusinessUnitName = "Business Unit B",
                FreezePriceFlag = true,
                FreezeShippingChargeFlag = false,
                FreezeTaxFlag = true,
                SubmittedFlag = true,
                SourceTransactionRevisionNumber = 1,
                BillToCustomer = new CustomerBill
                {
                    AccountNumber = 12345
                },
                ShipToCustomer = new CustomerShip
                {
                    PartyNumber = 6789
                },
                Lines = new List<OrderLines> {
                    new OrderLines
                    {
                        SourceTransactionLineId = 1,
                        SourceTransactionLineNumber = 1,
                        SourceTransactionScheduleId = 1,
                        SourceScheduleNumber = 1,
                        TransactionCategoryCode = "Category A",
                        TransactionLineType = "Type B",
                        ProductNumber = "Product 1",
                        OrderedQuantity = 2,
                        OrderedUOM = "EA"
                    },
                    new OrderLines
                    {
                        SourceTransactionLineId = 2,
                        SourceTransactionLineNumber = 2,
                        SourceTransactionScheduleId = 2,
                        SourceScheduleNumber = 2,
                        TransactionCategoryCode = "Category B",
                        TransactionLineType = "Type C",
                        ProductNumber = "Product 2",
                        OrderedQuantity = 3,
                        OrderedUOM = "EA"
                    }
                }
            };

            var result = await _client.CreateOrder(order, CancellationToken.None);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            result.Message.Should().Be("Bad Request");
        }
    }
}