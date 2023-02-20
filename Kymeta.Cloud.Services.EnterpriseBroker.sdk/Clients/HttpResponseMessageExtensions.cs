using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.REST;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;

namespace Kymeta.Cloud.Services.EnterpriseBroker.HttpClients
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<OracleResponse<CreateOrderResponse>> ProcessResponseFromOracle(this HttpResponseMessage response, CancellationToken token)
        {
            string content = await response.Content.ReadAsStringAsync(token);
           
            return new OracleResponse<CreateOrderResponse>(response.StatusCode, response.ReasonPhrase, content);
        }
    }
}
