using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.REST;
using System.Text.Json;
using System.Text;
using Kymeta.Cloud.Services.EnterpriseBroker.Models;
using System.Configuration;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.Orders;
using Microsoft.VisualBasic;

namespace Kymeta.Cloud.Services.EnterpriseBroker.HttpClients
{
    public interface IOracleClient
    {
        Task<Tuple<OracleAddressObject, string>> CreateAddress(string accountNumber, CreateOracleAddressViewModel model);
        Task<Tuple<OracleOrganizationResponse, string>> CreateOrganization(CreateOracleOrganizationModel model);
        Task<Tuple<OracleAddressObject, string>> UpdateAddress(string accountNumber, CreateOracleAddressViewModel model, string partyNumber);
        Task<OracleResponse> CreateOrder(CreateOrder newOrder, CancellationToken cancellationToken);
    }

    public class OracleClient : IOracleClient
    {
        private const string UserName = "Oracle:Username";
        private const string Password = "Oracle:Password";
        private readonly HttpClient _client;
        private readonly ILogger<IOracleClient> _logger;

        public OracleClient(HttpClient client, IConfiguration configuration, ILogger<IOracleClient> logger)
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("Oracle:Endpoint"));
            // custom BasicAuthenticationHeaderValue class to wrap the encoding 
            client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(configuration.GetValue<string>(UserName), configuration.GetValue<string>(Password));
            _client = client;
            _logger = logger;
        }

        public async Task<Tuple<OracleOrganizationResponse, string>> CreateOrganization(CreateOracleOrganizationModel model)
        {
            var response = await _client.PostAsJsonAsync($"crmRestApi/resources/latest/accounts", model, new JsonSerializerOptions { PropertyNameCaseInsensitive = false });
            string data = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogCritical($"Failed CreateOrganization Oracle HTTP call: {(int)response.StatusCode} | {data} | Model sent: {JsonSerializer.Serialize(model)}");
                return new Tuple<OracleOrganizationResponse, string>(null, data);
            }

            var deserializedObject = JsonSerializer.Deserialize<OracleOrganizationResponse>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new Tuple<OracleOrganizationResponse, string>(deserializedObject, null);
        }

        public async Task<Tuple<OracleAddressObject, string>> CreateAddress(string accountNumber, CreateOracleAddressViewModel model)
        {
            var response = await _client.PostAsJsonAsync($"crmRestApi/resources/latest/accounts/{accountNumber}/child/Address", model, new JsonSerializerOptions { PropertyNameCaseInsensitive = false });
            string data = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogCritical($"Failed AddAddress Oracle HTTP call: {(int)response.StatusCode} | {data} | Model sent: {JsonSerializer.Serialize(model)}");
                return new Tuple<OracleAddressObject, string>(null, data);
            }

            var deserializedObject = JsonSerializer.Deserialize<OracleAddressObject>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new Tuple<OracleAddressObject, string>(deserializedObject, null);
        }


        public async Task<Tuple<OracleAddressObject, string>> UpdateAddress(string accountNumber, CreateOracleAddressViewModel model, string partyNumber)
        {
            var serialized = JsonSerializer.Serialize(model, new JsonSerializerOptions { PropertyNameCaseInsensitive = false });
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            var response = await _client.PatchAsync($"crmRestApi/resources/latest/accounts/{accountNumber}/child/Address/{partyNumber}", content);

            string data = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogCritical($"Failed UpdateAddress Oracle HTTP call: {(int)response.StatusCode} | {data} | Model sent: {JsonSerializer.Serialize(model)}");
                return new Tuple<OracleAddressObject, string>(null, data);
            }

            var deserializedObject = JsonSerializer.Deserialize<OracleAddressObject>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new Tuple<OracleAddressObject, string>(deserializedObject, null);
        }

        public async Task<OracleResponse> CreateOrder(CreateOrder newOrder, CancellationToken cancellationToken)
        {
            var content = new StringContent(JsonSerializer.Serialize(newOrder), Encoding.UTF8, "application/json");
           
            HttpResponseMessage response = await _client.PostAsync("/fscmRestApi/resources/11.13.18.05/salesOrdersForOrderHub", content, cancellationToken);
            
            return  await response.ProcessResponseFromOracle(cancellationToken);
        }
    }
}