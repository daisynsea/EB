namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.Orders
{
    public class CreateOrder
    {
        public string SourceTransactionNumber { get; set; }
        public string SourceTransactionId { get; set; }
        public string OrderKey { get; set; }
        public string SourceTransactionSystem { get; set; }
        public string BusinessUnitName { get; set; }
        public string BuyingPartyNumber { get; set; }
        public string BuyingPartyContactNumber { get; set; }
        public string TransactionType { get; set; }
        public string RequestedShipDate { get; set; }
        public string PaymentTerms { get; set; }
        public string TransactionalCurrencyCode { get; set; }
        public string RequestingBusinessUnitName { get; set; }
        public bool FreezePriceFlag { get; set; }
        public bool FreezeShippingChargeFlag { get; set; }
        public bool FreezeTaxFlag { get; set; }
        public bool SubmittedFlag { get; set; }
        public long SourceTransactionRevisionNumber { get; set; }
        public CustomerBill BillToCustomer { get; set; } = null!;
        public CustomerShip ShipToCustomer { get; set; } = null!;

        public IEnumerable<OrderLines> Lines { get; set; } = null!;
    }

    public class OrderLines
    {
        public long SourceTransactionLineId { get; set; }
        public long SourceTransactionLineNumber { get; set; }
        public long SourceTransactionScheduleId { get; set; }
        public long SourceScheduleNumber { get; set; }
        public string TransactionCategoryCode { get; set; }
        public string TransactionLineType { get; set; }
        public string ProductNumber { get; set; }
        public long OrderedQuantity { get; set; }
        public string OrderedUOM { get; set; }
    }

    public class CustomerBill
    {
        public long AccountNumber { get; set; }
        //public long ContactNumber { get; set; }
        //public string AddressId { get; set; }
    }


    public class CustomerShip
    {
        public long PartyNumber { get; set; }
        //public long ContactNumber { get; set; }
        //public string AddressId { get; set; }
    }
}