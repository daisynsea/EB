using Kymeta.Cloud.Services.EnterpriseBroker.Models.OSS;
using System.Text.Json;
using System.Web;

namespace Kymeta.Cloud.Services.EnterpriseBroker.HttpClients;

/// <summary>
/// Client used to communicate with the Terminals microservice
/// </summary>
public interface ITerminalsClient
{
    Task<(TerminalsResponse res, string error)> GetTerminalsBySalesforceIds(IEnumerable<string> salesforceIds);
    Task<(Terminal updated, string error)> UpdateTerminal(Terminal terminal);
    Task<(IEnumerable<ProductType> productTypes, string error)> GetProductTypes();
}
public class TerminalsClient : ITerminalsClient
{
    private readonly HttpClient _client;
    private readonly ILogger<TerminalsClient> _logger;

    public TerminalsClient(HttpClient client, IConfiguration config, ILogger<TerminalsClient> logger)
    {
        client.BaseAddress = new Uri(config["Api:Terminals"]);
        client.DefaultRequestHeaders.Add("sharedKey", config["SharedKey"]);

        _client = client;
        _logger = logger;
    }

    public async Task<(TerminalsResponse res, string error)> GetTerminalsBySalesforceIds(IEnumerable<string> salesforceIds)
    {
        // exit early if no serials were provided
        if (salesforceIds == null || !salesforceIds.Any()) return new ValueTuple<TerminalsResponse, string>(null, $"No Salesforce Ids provided.");

        // join the list together to formulate the query
        var terminalsQuery = string.Join('|', salesforceIds);
        // send the request
        var response = await _client.GetAsync($"v1/?take=25&filters=SalesforceAssetId:{terminalsQuery}");
        string data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical($"Failed GetTerminalsBySalesforceIds HTTP call: {(int)response.StatusCode} | {data} | Salesforce Ids sent: {JsonSerializer.Serialize(salesforceIds)}");
            return new ValueTuple<TerminalsResponse, string>(null, data);
        }

        var result = JsonSerializer.Deserialize<TerminalsResponse>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return new ValueTuple<TerminalsResponse, string>(result, null);
    }

    public async Task<(Terminal updated, string error)> UpdateTerminal(Terminal terminal)
    {
        // exit early if no serials were provided
        if (terminal == null) return new ValueTuple<Terminal, string>(null, $"Terminal not provided.");

        var response = await _client.PutAsJsonAsync($"v1/{terminal.Id}", terminal);
        string data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical($"Failed UpdateTerminal HTTP call: {(int)response.StatusCode} | {data} | Serials sent: {JsonSerializer.Serialize(terminal)}");
            return new ValueTuple<Terminal, string>(null, data);
        }

        var result = JsonSerializer.Deserialize<Terminal>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return new ValueTuple<Terminal, string>(result, null);
    }

    public async Task<(IEnumerable<ProductType> productTypes, string error)> GetProductTypes()
    {
        var response = await _client.GetAsync($"v1/info/products/types");
        string data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical($"Failed GetProductTypes HTTP call: {(int)response.StatusCode} | {data}");
            return new ValueTuple<IEnumerable<ProductType>, string>(null, data);
        }

        var result = JsonSerializer.Deserialize<IEnumerable<ProductType>>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return new ValueTuple<IEnumerable<ProductType>, string>(result, null);
    }
}