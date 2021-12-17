using System.Text.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.HttpClients;

/// <summary>
/// Client used to communicate with the Accounts microservice
/// </summary>
public interface IAccountsClient
{
    Task<Tuple<Account, string>> AddAccount(Account model);
    Task<Tuple<Account, string>> UpdateAccount(Guid id, Account model);
    Task<Account> GetAccountBySalesforceId(string sfid);
}
public class AccountsClient : IAccountsClient
{
    private readonly HttpClient _client;
    private readonly ILogger<AccountsClient> _logger;

    public AccountsClient(HttpClient client, IConfiguration config, ILogger<AccountsClient> logger)
    {
        client.BaseAddress = new Uri(config["Api:Accounts"]);
        client.DefaultRequestHeaders.Add("sharedKey", config["SharedKey"]);

        _client = client;
        _logger = logger;
    }

    public async Task<Tuple<Account, string>> AddAccount(Account model)
    {
        var response = await _client.PostAsJsonAsync($"v1", model);
        string data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical($"Failed AddAccount HTTP call: {(int)response.StatusCode} | {data} | Model sent: {JsonSerializer.Serialize(model)}");
            return new Tuple<Account, string>(null, data);
        }
        
        return new Tuple<Account, string>(JsonSerializer.Deserialize<Account>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }), null);

    }

    public async Task<Tuple<Account, string>> UpdateAccount(Guid id, Account model)
    {
        var response = await _client.PutAsJsonAsync($"v1/{id}", model);
        string data = await response.Content?.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical($"Failed UpdateAccount HTTP call: {(int)response.StatusCode} | {data} | Model sent: {JsonSerializer.Serialize(model)}");
            return new Tuple<Account, string>(null, data);
        }

        return new Tuple<Account, string>(JsonSerializer.Deserialize<Account>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }), null);

    }

    public async Task<Account> GetAccountBySalesforceId(string sfid)
    {
        var response = await _client.GetAsync($"v1/sfid/{sfid}");
        string data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode) return null;

        return JsonSerializer.Deserialize<Account>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}