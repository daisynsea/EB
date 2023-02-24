
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Newtonsoft.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders
{
    public class SalesforceOrder
    {
        [JsonIgnore]
        public string? Id { get; set; }
        [JsonIgnore]
        public string? OrderNumber { get; set; }
        [JsonIgnore]
        public string? BusinessUnit { get; set; }
        [JsonIgnore]
        public string? OrderType { get; set; }
        [JsonIgnore]
        public string? AccountName { get; set; }
        [JsonIgnore]
        public string? PrimaryContact { get; set; }
        [JsonIgnore]
        public string? PreferredContactMethod { get; set; }
        [JsonIgnore]
        public DateOnly? PoDate { get; set; }
        [JsonIgnore]
        public string? PoNumber { get; set; }
        [JsonIgnore]
        public string? BillToName { get; set; }
        [JsonIgnore]
        public string? OracleAccountId { get; set; }
        [JsonIgnore]
        public string? ShipToName { get; set; }
        [JsonIgnore]
        public string? ShippingAddress { get; set; }
        [JsonIgnore]
        public string? SalesRepresentative { get; set; }

        [JsonProperty("records")]
        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

        public bool IsValid()
        {
            return Id.IsNotEmpty();
              
        }
    }

    public class OrderProduct
    {
        public string Id { get; set; }
        public string? OrderId { get; set; }
        public string? Product_Code__c { get; set; }
        public string Product2Id { get; set; }
        public bool IsDeleted { get; set; }
        public object OriginalOrderItemId { get; set; }
        public double Quantity { get; set; }
        public string SBQQ__BillingFrequency__c { get; set; }
        public string? Number_of_Billing_Periods__c { get; set; }
        public double UnitPrice { get; set; }
        public string? NetPrice__c { get; set; }
        public string? ShipToContactId { get; set; }
        public string? Preferred_Contact_Method__c { get; set; }
        public string? Ship_Date__c { get; set; }
        public string? Shipping_Terms__c { get; set; }
        public string? Shipping_Details__c { get; set; }
        public string? Packing_Instructions__c { get; set; }
        public string? BillToAddress__c { get; set; }
        public string? BillToContactId { get; set; }
        public string? SBQQ__PaymentTerm__c { get; set; }
        public string? Sync_Instructions__c { get; set; }
        public string? Approved__c { get; set; }
        public string? OrderNumber { get; set; }
        public string? Oracle_Invoice__c { get; set; }
    }
}
