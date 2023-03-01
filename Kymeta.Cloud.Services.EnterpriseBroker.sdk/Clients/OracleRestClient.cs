using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;
using System.Net.Http.Headers;
using System.Net.Http;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients
{
    public interface IOracleRestClient
    {
        Task<OracleResponse<CreateOrderResponse>> CreateOrder(OracleCreateOrder newOrder, CancellationToken cancellationToken);
        Task<OracleResponse<GetOrderResponse>> GetOrder(string? orderKey, CancellationToken cancellationToken);
        Task<OracleResponse<UpdateOrderResponse>> UpdateOrder(OracleUpdateOrder newOrder, CancellationToken cancellationToken);
    }

    public class OracleRestClient : IOracleRestClient
    {
        private const string RequestUri = "fscmRestApi/resources/11.13.18.05/salesOrdersForOrderHub";
        private readonly HttpClient _client;
        public OracleRestClient(HttpClient client, ServiceOption option)
        {
            _client = client;
            SetupClient(option);
        }

        public async Task<OracleResponse<CreateOrderResponse>> CreateOrder(OracleCreateOrder newOrder, CancellationToken cancellationToken)
        {
           
            HttpResponseMessage response = await _client.PostAsync(RequestUri, SerializeToJsonString(newOrder), cancellationToken);

            return await response.ProcessResponseFromOracle<CreateOrderResponse>(cancellationToken);
        }


        public async Task<OracleResponse<GetOrderResponse>> GetOrder(string? orderKey, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await _client.GetAsync($"{RequestUri}?q=OrderNumber={orderKey}", cancellationToken);
            return await response.ProcessResponseFromOracle<GetOrderResponse>(cancellationToken);
        }

        public async Task<OracleResponse<UpdateOrderResponse>> UpdateOrder(OracleUpdateOrder newOrder, CancellationToken cancellationToken)
        {
            ActivateUpsertMode();
            HttpResponseMessage response = await _client.PatchAsync($"{RequestUri}/{newOrder.OrderKey}", SerializeToJsonString(newOrder), cancellationToken);
            DeactivateUpsertMode(); //check about this
            return await response.ProcessResponseFromOracle<UpdateOrderResponse>(cancellationToken);
        }

        private static StringContent SerializeToJsonString<T>(T instance)
        {
            var serialized = JsonSerializer.Serialize(instance, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new StringContent(serialized, Encoding.UTF8, "application/json");
        }

        private void SetupClient(ServiceOption option)
        {
            _client.BaseAddress = new Uri(option.Oracle.Endpoint);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string basicAuth = Convert.ToBase64String($"{option.Oracle.Username}:{option.Oracle.Password}".ToBytes(), Base64FormattingOptions.None);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
        }

        private void ActivateUpsertMode()
        {
            _client.DefaultRequestHeaders.Add("Upsert-Mode", "true");
        }
        private void DeactivateUpsertMode()
        {
            _client.DefaultRequestHeaders.Remove("Upsert-Mode");
        }
    }
}
