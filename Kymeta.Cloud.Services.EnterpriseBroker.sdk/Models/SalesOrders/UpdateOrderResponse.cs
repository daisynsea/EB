using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.REST;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders
{
    public class UpdateOrderResponse : IOracleResponsePayload
    {
        public string? OrderKey { get; set; }
        public string PackingInstructions { get; set; }
        public string ShippingInstructions { get; set; }
        public string FOBPointCode { get; set; }
        public bool IsSuccessfulResponse()
        {
            return true;
        }
    }
}