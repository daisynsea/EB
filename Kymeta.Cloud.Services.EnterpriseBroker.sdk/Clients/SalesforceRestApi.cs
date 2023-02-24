
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;

public interface ISalesforceRestApi
{
    Task<IEnumerable<OrderProduct>> GetOrderProducts(string orderNumber, CancellationToken cancellationToken);
}

public class SalesforceRestApi : ISalesforceRestApi
{
    private readonly HttpClient _client;
    private readonly ILogger<ISalesforceRestApi> _logger;

    public SalesforceRestApi(HttpClient client, ILogger<ISalesforceRestApi> logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
    }

    public async Task<IEnumerable<OrderProduct>> GetOrderProducts(string orderKey, CancellationToken cancellationToken)
    {
        //var limitToUnsyncedOrders = " and NEO_Sync_to_Oracle__c=false";
        var query = $"select FIELDS(ALL) from OrderItem where orderId='{orderKey}' LIMIT 200"; // limitToUnsynced should we here after it's provisioned on SF env

        HttpResponseMessage response = await _client.GetAsync($"/services/data/v56.0/query?q={query}", cancellationToken);
        string content = await response.Content.ReadAsStringAsync(cancellationToken);
        var orderSF = JsonConvert.DeserializeObject<SalesforceOrder>(content);
        return orderSF.OrderProducts;
    }
}
