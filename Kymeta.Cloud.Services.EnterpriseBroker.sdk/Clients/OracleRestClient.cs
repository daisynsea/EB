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
        private readonly HttpClient _client;
        private readonly ILogger<IOracleRestClient> _logger;
        public OracleRestClient(HttpClient client, ILogger<IOracleRestClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<OracleResponse<CreateOrderResponse>> CreateOrder(OracleCreateOrder newOrder, CancellationToken cancellationToken)
        {
            var serialized = JsonSerializer.Serialize(newOrder, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync("fscmRestApi/resources/11.13.18.05/salesOrdersForOrderHub", content, cancellationToken);

            return await response.ProcessResponseFromOracle<CreateOrderResponse>(cancellationToken);
        }
    }
}
