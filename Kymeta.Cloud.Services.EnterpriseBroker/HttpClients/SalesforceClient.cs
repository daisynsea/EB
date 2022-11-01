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
    Task<SalesforceProductReportResultModel> GetProductReportFromSalesforce();
    Task<IEnumerable<SalesforceProductObjectModelV2>> GetProductsByManyIds(IEnumerable<string> productIds);
    Task<SalesforceQueryObjectModel> GetRelatedFiles(IEnumerable<string> objectIds);
    Task<SalesforceFileResponseModel?> GetFileMetadataByManyIds(IEnumerable<string> fileIds);
    Task<Stream> DownloadFileContent(string downloadUrl);
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
    public async Task<SalesforceProductReportResultModel> GetProductReportFromSalesforce()
    {
        try
        {
            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;
            var productsReportId = _config["Salesforce:ProductsReportId"];

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await _client.GetAsync($"{url}/services/data/v53.0/analytics/reports/{productsReportId}"); //prod : 00O3j000007n2CREAY, cloudDev : 00O0r000000SnZtEAK
            var stringResponse = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return null;

            var productObject = JsonConvert.DeserializeObject<SalesforceProductReportResultModel>(stringResponse);

            return productObject;

        }
        catch (Exception ex)
        {
            _logger.LogError($"[EB] Exception thrown when fetching Products Report from Salesforce: {ex.Message}");
            return null;
        }
    }
    public async Task<IEnumerable<SalesforceProductObjectModelV2>> GetProductsByManyIds(IEnumerable<string> productIds)
    {
        try
        {
            // if no productIds were provided, return empty list
            if (productIds == null || !productIds.Any()) return new List<SalesforceProductObjectModelV2>();

            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;

            // define the payload the Salesforce endpoint accepts
            var payload = new SalesforceProductCompositeObjectModel()
            {
                Ids = productIds.ToList(),
                Fields = new List<string>()
                {
                    "Id",
                    "Name__c",
                    "ItemDetails__c",
                    "Terminal_Category__c"
                }
            };

            // append auth token
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            // send the request
            var response = await _client.PostAsJsonAsync($"{url}/services/data/v53.0/composite/sobjects/Product2", payload);
            var stringResponse = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                // the request failed
                _logger.LogError($"The attempt to fetch Products from Salesforce failed: {stringResponse}", productIds);
                return null;
            }

            var products = JsonConvert.DeserializeObject<List<SalesforceProductObjectModelV2>>(stringResponse);
            return products;

        }
        catch (Exception ex)
        {
            _logger.LogError($"[EB] Exception thrown when fetching Products from Salesforce: {ex.Message}");
            return null;
        }
    }
    public async Task<SalesforceQueryObjectModel> GetRelatedFiles(IEnumerable<string> objectIds)
    {
        try
        {
            // if no objectIds were provided, return empty list
            if (objectIds == null || !objectIds.Any()) return null;

            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;

            // TODO: need to accommodate greater than 100 items in URL query
            if (objectIds.Count() > 100) throw new ArgumentOutOfRangeException(nameof(objectIds));

            var formattedIds = objectIds.Select(id => $"'{id}'");
            var payload = string.Join(",", formattedIds);

            // append auth token
            if (!_client.DefaultRequestHeaders.Any(drh => drh.Key == "Authorization")) _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            // send the request
            var response = await _client.GetAsync($"{url}/services/data/v43.0/query?q=select id, LinkedEntityId,ContentDocumentId from ContentDocumentLink where LinkedEntityId IN ({payload})");
            var stringResponse = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                // the request failed
                _logger.LogError($"The attempt to fetch Related Files from Salesforce failed: {stringResponse}", objectIds);
                return null;
            }
            var result = JsonConvert.DeserializeObject<SalesforceQueryObjectModel>(stringResponse);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"[EB] Exception thrown when fetching Related Files from Salesforce: {ex.Message}", objectIds);
            throw;
        }
    }
    public async Task<SalesforceFileResponseModel?> GetFileMetadataByManyIds(IEnumerable<string> fileIds)
    {
        try
        {
            // if no objectIds were provided, return empty list
            if (fileIds == null || !fileIds.Any()) return null;

            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;

            // TODO: do we need to accommodate greater than 100 items in URL query?
            if (fileIds.Count() > 100) throw new ArgumentOutOfRangeException(nameof(fileIds));

            var payload = string.Join(",", fileIds);

            // append auth token
            if (!_client.DefaultRequestHeaders.Any(drh => drh.Key == "Authorization")) _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            // send the request
            var response = await _client.GetAsync($"{url}/services/data/v43.0/connect/files/batch/{payload}");
            var stringResponse = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                // the request failed
                _logger.LogError($"The attempt to fetch Files from Salesforce failed: {stringResponse}", fileIds);
                return null;
            }

            var files = JsonConvert.DeserializeObject<SalesforceFileResponseModel>(stringResponse);
            return files;

        }
        catch (Exception ex)
        {
            _logger.LogError($"[EB] Exception thrown when fetching Files from Salesforce: {ex.Message}");
            return null;
        }
    }
    public async Task<Stream> DownloadFileContent(string downloadUrl)
    {
        try
        {
            // if no URL was provided, return null
            if (string.IsNullOrEmpty(downloadUrl)) return null;

            var tokenAndUrl = await GetTokenAndUrl();
            var token = tokenAndUrl?.Item1;
            var url = tokenAndUrl?.Item2;

            // append auth token
            if (!_client.DefaultRequestHeaders.Any(drh => drh.Key == "Authorization")) _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            // send the request
            var response = await _client.GetAsync($"{url}/{downloadUrl}");
            if (!response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                // the request failed
                _logger.LogError($"The attempt to fetch File content from Salesforce failed: {stringResponse}", downloadUrl);
                return null;
            }
            var streamResponse = await response.Content.ReadAsStreamAsync();
            return streamResponse;

        }
        catch (Exception ex)
        {
            _logger.LogError($"[EB] Exception thrown when fetching Files from Salesforce: {ex.Message}");
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

