
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Kymeta.Cloud.Services.Toolbox.Rest;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;

public interface ISalesforceRestClient
{
    Task<SalesOrderLineResponse> GetOrderProducts(string orderNumber, CancellationToken cancellationToken);
    Task<HttpStatusCode> SyncFromOracle( OracleSalesforceSyncRequest syncRequest, CancellationToken cancellationToken);
}

public class SalesforceRestClient : ISalesforceRestClient
{
    private readonly HttpClient _client;
    private readonly ILogger<ISalesforceRestClient> _logger;

    public SalesforceRestClient(HttpClient client, ILogger<ISalesforceRestClient> logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
    }

    public async Task<SalesOrderLineResponse> GetOrderProducts(string orderKey, CancellationToken cancellationToken)
    {
        var sfOrder = await new RestClient(_client)
            .SetPath($"query?q={CreateQueryToGetProducts(orderKey)}")
            .SetLogger(_logger)
            .GetAsync(cancellationToken)
            .GetRequiredContent<SalesOrderLineResponse>();
      
        return sfOrder;
    }

    public async Task<HttpStatusCode> SyncFromOracle(OracleSalesforceSyncRequest syncRequest, CancellationToken cancellationToken)
    {
        var response = await new RestClient(_client)
        .SetPath("composite")
        .SetLogger(_logger)
        .GetAsync(cancellationToken);
        return response.HttpResponseMessage.StatusCode;
    }

    private string CreateQueryToGetProducts(string orderKey)
    {
        var filedsToGet = "Id, Product_Code__c, Quantity, OrderItemNumber, OrderId, NEO_Sync_to_Oracle__c, UnitPrice";
        var limitToUnsyncedOrders = " and NEO_Sync_to_Oracle__c=true";
        var query = $"select {filedsToGet} from OrderItem where OrderId='{orderKey}' {limitToUnsyncedOrders}";
        return query;
    }


}
