using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients
{
    public interface IOracleRestClient
    {
        Task<OracleResponse<CreateOrderResponse>> CreateOrder(OracleCreateOrder newOrder, CancellationToken cancellationToken);
    }

    public class OracleRestClient : IOracleRestClient
    {
        private const string RequestUri = "fscmRestApi/resources/11.13.18.05/salesOrdersForOrderHub";
        private readonly HttpClient _client;
        public OracleRestClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<OracleResponse<CreateOrderResponse>> CreateOrder(OracleCreateOrder newOrder, CancellationToken cancellationToken)
        {

            HttpResponseMessage response = await _client.PostAsync(RequestUri, SerializeToJsonString(newOrder), cancellationToken);

            return await response.ProcessResponseFromOracle<CreateOrderResponse>(cancellationToken);
        }

        private static StringContent SerializeToJsonString(OracleCreateOrder newOrder)
        {
            var serialized = JsonSerializer.Serialize(newOrder, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new StringContent(serialized, Encoding.UTF8, "application/json");
        }
    }
}
