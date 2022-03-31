using Kymeta.Cloud.Commons.Databases.Redis;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;
using Newtonsoft.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.HttpClients;

public interface ISalesforceClient
{
    Task<SalesforceAddressObjectModel> GetAddressFromSalesforce(string addressId);
    Task<SalesforceContactObjectModel> GetContactFromSalesforce(string contactId);
    Task<SalesforceAccountObjectModel> GetAccountFromSalesforce(string accountId);
    Task<SalesforceUserObjectModel> GetUserFromSalesforce(string userId);
}

public class SalesforceClient : ISalesforceClient
{
    private HttpClient _client;
    private IConfiguration _config;
    private ILogger<UsersClient> _logger;
    private IRedisClient _redis;

    public SalesforceClient(HttpClient client, IConfiguration config, ILogger<UsersClient> logger, IRedisClient redis)
    {
        _client = client;
        _config = config;
        _logger = logger;
        _redis = redis;
    }

    public async Task<SalesforceAddressObjectModel> GetAddressFromSalesforce(string addressId)
    {
        try
        {
            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await _client.GetAsync($"{url}/services/data/v53.0/sobjects/Address__c/{addressId}");
            var stringResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            var accountObject = JsonConvert.DeserializeObject<SalesforceAddressObjectModel>(stringResponse);

            return accountObject;

        }
        catch (Exception ex)
        {
            _logger.LogError($"[EB] Exception thrown when fetching Address from Salesforce: {ex.Message}");
            return null;
        }
    }

    public async Task<SalesforceContactObjectModel> GetContactFromSalesforce(string contactId)
    {
        try
        {
            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await _client.GetAsync($"{url}/services/data/v53.0/sobjects/Contact/{contactId}");
            var stringResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            var accountObject = JsonConvert.DeserializeObject<SalesforceContactObjectModel>(stringResponse);

            return accountObject;

        }
        catch (Exception ex)
        {
            _logger.LogError($"[EB] Exception thrown when fetching Contact from Salesforce: {ex.Message}");
            return null;
        }
    }

    public async Task<SalesforceAccountObjectModel> GetAccountFromSalesforce(string accountId)
    {
        try
        {
            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await _client.GetAsync($"{url}/services/data/v53.0/sobjects/Account/{accountId}");
            var stringResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            var accountObject = JsonConvert.DeserializeObject<SalesforceAccountObjectModel>(stringResponse);

            return accountObject;

        } catch (Exception ex)
        {
            _logger.LogError($"[EB] Exception thrown when fetching Account from Salesforce: {ex.Message}");
            return null;
        }
    }

    public async Task<SalesforceUserObjectModel> GetUserFromSalesforce(string userId)
    {
        try
        {
            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await _client.GetAsync($"{url}/services/data/v53.0/sobjects/User/{userId}");
            var stringResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            var accountObject = JsonConvert.DeserializeObject<SalesforceUserObjectModel>(stringResponse);

            return accountObject;

        }
        catch (Exception ex)
        {
            _logger.LogError($"[EB] Exception thrown when fetching User from Salesforce: {ex.Message}");
            return null;
        }
    }

    private async Task<Tuple<string, string>?> GetTokenAndUrl()
    {
        var token = _redis.StringGet<string>("EB:SFToken");
        var url = _redis.StringGet<string>("EB:SFApiRoot");

        // authenticate
        if (string.IsNullOrEmpty(token))
        {
            var authRequest = new SalesforceAuthenticationRequest
            {
                ClientId = _config["Salesforce:ConnectedApp:ClientId"],
                ClientSecret = _config["Salesforce:ConnectedApp:ClientSecret"],
                GrantType = "password",
                Username = _config["Salesforce:Username"],
                Password = _config["Salesforce:Password"]
            };
            var authObject = await Authenticate(authRequest);

            if (authObject != null)
            {
                token = authObject.AccessToken;
                url = authObject.InstanceUrl;
            }
            else
            {
                _logger.LogError($"[EB] Attempted to log into Salesforce, but failed to get token. First 6 of client id: {authRequest.ClientId.Substring(0, 6)}, First 6 of client secret: {authRequest.ClientSecret.Substring(0, 6)}");
                return null;
            }
        }

        return new Tuple<string, string>(token, url);
    }

    private async Task<SalesforceAuthenticationResponse> Authenticate(SalesforceAuthenticationRequest request)
    {
        HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "password" },
            { "client_id", request.ClientId ?? "defaultclientid" },
            { "client_secret", request.ClientSecret ?? "defaultclientsecret" },
            { "username", request.Username ?? "defaultusername" },
            { "password", request.Password ?? "defaultpassword" }
        });

        var response = await _client.PostAsync($"{_config["Salesforce:LoginEndpoint"]}/services/oauth2/token", content);
        var stringResponse = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode) return null;

        var authenticationObject = JsonConvert.DeserializeObject<SalesforceAuthenticationResponse>(stringResponse);
        if (authenticationObject != null)
        {
            _redis.StringSet("EB:SFToken", authenticationObject?.AccessToken, TimeSpan.FromHours(1));
            _redis.StringSet("EB:SFApiRoot", authenticationObject?.InstanceUrl, TimeSpan.FromHours(1));
        }

        return authenticationObject;
    }
}

