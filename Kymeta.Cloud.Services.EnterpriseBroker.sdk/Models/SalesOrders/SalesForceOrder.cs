
namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders
{
    public class SalesForceOrder
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
