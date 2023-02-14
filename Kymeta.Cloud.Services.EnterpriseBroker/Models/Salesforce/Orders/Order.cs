using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.Orders;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.Orders
{
    public class Order
    {
        OrderProduct[] Products { get; set; } = Array.Empty<OrderProduct>();
    }

    public class OrderProduct 
    {
        public string? OrderId { get; set; }
        public string? BillingFrequency { get; set; }
        public string? NumberOfBillingPeriods { get; set; }
        public string? LineType { get; set; }
    }
}
