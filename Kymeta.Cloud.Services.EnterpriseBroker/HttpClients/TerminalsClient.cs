using System.Text.Json;
using System.Web;

namespace Kymeta.Cloud.Services.EnterpriseBroker.HttpClients;

/// <summary>
/// Client used to communicate with the Terminals microservice
/// </summary>
public interface ITerminalsClient
{
    Task<Tuple<TerminalsResponse<dynamic>, string>> GetTerminalsByAssetSerials(IEnumerable<string> serials);
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

    public async Task<Tuple<TerminalsResponse<dynamic>, string>> GetTerminalsByAssetSerials(IEnumerable<string> serials)
    {
        UriBuilder builder = new("v1");
        //builder.Port = -1;
        var query = HttpUtility.ParseQueryString(builder.Query);
        query["skip"] = "0";
        query["take"] = "50";
        query["query"] = "ABS865K210813282";
        builder.Query = query.ToString();
        string url = builder.ToString();

        // TODO: verify format of URL
        var response = await _client.GetAsync(url);
        string data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical($"Failed GetTerminalsByAssetSerials HTTP call: {(int)response.StatusCode} | {data} | Serials sent: {JsonSerializer.Serialize(serials)}");
            return new Tuple<TerminalsResponse<dynamic>, string>(null, data);
        }

        var result = JsonSerializer.Deserialize<TerminalsResponse<dynamic>>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });        
        return new Tuple<TerminalsResponse<dynamic>, string>(result, null);

    }
}