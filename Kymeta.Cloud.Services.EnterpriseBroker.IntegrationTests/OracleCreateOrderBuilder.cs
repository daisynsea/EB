
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;

namespace Kymeta.Cloud.Services.EnterpriseBroker.IntegrationTests
{
    public class OracleCreateOrderBuilder
    {
        private OracleCreateOrder order = new OracleCreateOrder();

        public OracleCreateOrderBuilder WithSourceTransactionNumber(string sourceTransactionNumber)
        {
            order.SourceTransactionNumber = sourceTransactionNumber;
            return this;
        }

        public OracleCreateOrderBuilder WithSourceTransactionId(string sourceTransactionId)
        {
            order.SourceTransactionId = sourceTransactionId;
            return this;
        }

        public OracleCreateOrderBuilder WithOrderKey(string orderKey)
        {
            order.OrderKey = orderKey;
            return this;
        }

        public OracleCreateOrderBuilder WithSourceTransactionSystem(string sourceTransactionSystem)
        {
            order.SourceTransactionSystem = sourceTransactionSystem;
            return this;
        }

        public OracleCreateOrderBuilder WithBusinessUnitName(string businessUnitName)
        {
            order.BusinessUnitName = businessUnitName;
            return this;
        }

        public OracleCreateOrderBuilder WithBuyingPartyNumber(string buyingPartyNumber)
        {
            order.BuyingPartyNumber = buyingPartyNumber;
            return this;
        }

        public OracleCreateOrderBuilder WithBuyingPartyContactNumber(string buyingPartyContactNumber)
        {
            order.BuyingPartyContactNumber = buyingPartyContactNumber;
            return this;
        }

        public OracleCreateOrderBuilder WithTransactionType(string transactionType)
        {
            order.TransactionType = transactionType;
            return this;
        }

        public OracleCreateOrderBuilder WithRequestedShipDate(string requestedShipDate)
        {
            order.RequestedShipDate = requestedShipDate;
            return this;
        }

        public OracleCreateOrderBuilder WithPaymentTerms(string paymentTerms)
        {
            order.PaymentTerms = paymentTerms;
            return this;
        }

        public OracleCreateOrderBuilder WithTransactionalCurrencyCode(string transactionalCurrencyCode)
        {
            order.TransactionalCurrencyCode = transactionalCurrencyCode;
            return this;
        }

        public OracleCreateOrderBuilder WithRequestingBusinessUnitName(string requestingBusinessUnitName)
        {
            order.RequestingBusinessUnitName = requestingBusinessUnitName;
            return this;
        }

        public OracleCreateOrderBuilder WithFreezePriceFlag(bool freezePriceFlag)
        {
            order.FreezePriceFlag = freezePriceFlag;
            return this;
        }

        public OracleCreateOrderBuilder WithFreezeShippingChargeFlag(bool freezeShippingChargeFlag)
        {
            order.FreezeShippingChargeFlag = freezeShippingChargeFlag;
            return this;
        }

        public OracleCreateOrderBuilder WithFreezeTaxFlag(bool freezeTaxFlag)
        {
            order.FreezeTaxFlag = freezeTaxFlag;
            return this;
        }

        public OracleCreateOrderBuilder WithSubmittedFlag(bool submittedFlag)
        {
            order.SubmittedFlag = submittedFlag;
            return this;
        }

        public OracleCreateOrderBuilder WithSourceTransactionRevisionNumber(string sourceTransactionRevisionNumber)
        {
            order.SourceTransactionRevisionNumber = sourceTransactionRevisionNumber;
            return this;
        }

        public OracleCreateOrderBuilder WithBillToCustomer(string accountNumber)
        {
            order.BillToCustomer = new[] { new CustomerBill
            {
                AccountNumber = accountNumber
            } };
            return this;
        }

        public OracleCreateOrderBuilder WithShipToCustomer(string partyNumber)
        {
            order.ShipToCustomer = new[] { new CustomerShip
            {
                PartyNumber = partyNumber
            } };
            return this;
        }

        public OracleCreateOrderBuilder WithShipToCustomer(CustomerShip customerShip)
        {
            order.ShipToCustomer = new[] { customerShip };
            return this;
        }

        public OracleCreateOrderBuilder WithOrderLines(IEnumerable<OrderLines> orderLines)
        {
            order.Lines = orderLines.ToArray();
            return this;
        }

        public OracleCreateOrder Build()
        {
            return order;
        }

        public OracleCreateOrderBuilder WithBillToCustomer(CustomerBill customerBill)
        {
            order.BillToCustomer = new[] { customerBill };
            return this;
        }

        internal OracleCreateOrderBuilder WithLines(List<OrderLines> orderLines)
        {
            order.Lines = orderLines.ToArray();
            return this;
        }
    }
}
