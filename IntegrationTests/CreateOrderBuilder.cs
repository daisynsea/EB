using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.Orders;

namespace Kymeta.Cloud.Services.EnterpriseBroker.IntegrationTests
{
    public class CreateOrderBuilder
    {
        private CreateOrder order = new CreateOrder();

        public CreateOrderBuilder WithSourceTransactionNumber(string sourceTransactionNumber)
        {
            order.SourceTransactionNumber = sourceTransactionNumber;
            return this;
        }

        public CreateOrderBuilder WithSourceTransactionId(string sourceTransactionId)
        {
            order.SourceTransactionId = sourceTransactionId;
            return this;
        }

        public CreateOrderBuilder WithOrderKey(string orderKey)
        {
            order.OrderKey = orderKey;
            return this;
        }

        public CreateOrderBuilder WithSourceTransactionSystem(string sourceTransactionSystem)
        {
            order.SourceTransactionSystem = sourceTransactionSystem;
            return this;
        }

        public CreateOrderBuilder WithBusinessUnitName(string businessUnitName)
        {
            order.BusinessUnitName = businessUnitName;
            return this;
        }

        public CreateOrderBuilder WithBuyingPartyNumber(string buyingPartyNumber)
        {
            order.BuyingPartyNumber = buyingPartyNumber;
            return this;
        }

        public CreateOrderBuilder WithBuyingPartyContactNumber(string buyingPartyContactNumber)
        {
            order.BuyingPartyContactNumber = buyingPartyContactNumber;
            return this;
        }

        public CreateOrderBuilder WithTransactionType(string transactionType)
        {
            order.TransactionType = transactionType;
            return this;
        }

        public CreateOrderBuilder WithRequestedShipDate(string requestedShipDate)
        {
            order.RequestedShipDate = requestedShipDate;
            return this;
        }

        public CreateOrderBuilder WithPaymentTerms(string paymentTerms)
        {
            order.PaymentTerms = paymentTerms;
            return this;
        }

        public CreateOrderBuilder WithTransactionalCurrencyCode(string transactionalCurrencyCode)
        {
            order.TransactionalCurrencyCode = transactionalCurrencyCode;
            return this;
        }

        public CreateOrderBuilder WithRequestingBusinessUnitName(string requestingBusinessUnitName)
        {
            order.RequestingBusinessUnitName = requestingBusinessUnitName;
            return this;
        }

        public CreateOrderBuilder WithFreezePriceFlag(bool freezePriceFlag)
        {
            order.FreezePriceFlag = freezePriceFlag;
            return this;
        }

        public CreateOrderBuilder WithFreezeShippingChargeFlag(bool freezeShippingChargeFlag)
        {
            order.FreezeShippingChargeFlag = freezeShippingChargeFlag;
            return this;
        }

        public CreateOrderBuilder WithFreezeTaxFlag(bool freezeTaxFlag)
        {
            order.FreezeTaxFlag = freezeTaxFlag;
            return this;
        }

        public CreateOrderBuilder WithSubmittedFlag(bool submittedFlag)
        {
            order.SubmittedFlag = submittedFlag;
            return this;
        }

        public CreateOrderBuilder WithSourceTransactionRevisionNumber(string sourceTransactionRevisionNumber)
        {
            order.SourceTransactionRevisionNumber = sourceTransactionRevisionNumber;
            return this;
        }

        public CreateOrderBuilder WithBillToCustomer(string accountNumber)
        {
            order.BillToCustomer = new[] { new CustomerBill
            {
                AccountNumber = accountNumber
            } };
            return this;
        }

        public CreateOrderBuilder WithShipToCustomer(string partyNumber)
        {
            order.ShipToCustomer = new[] { new CustomerShip
            {
                PartyNumber = partyNumber
            } };
            return this;
        }

        public CreateOrderBuilder WithShipToCustomer(CustomerShip customerShip)
        {
            order.ShipToCustomer = new[] { customerShip };
            return this;
        }

        public CreateOrderBuilder WithOrderLines(IEnumerable<OrderLines> orderLines)
        {
            order.Lines = orderLines.ToArray();
            return this;
        }

        public CreateOrder Build()
        {
            return order;
        }

        public CreateOrderBuilder WithBillToCustomer(CustomerBill customerBill)
        {
            order.BillToCustomer = new[] { customerBill };
            return this;
        }

        internal CreateOrderBuilder WithLines(List<OrderLines> orderLines)
        {
            order.Lines = orderLines.ToArray();
            return this;
        }
    }
}
