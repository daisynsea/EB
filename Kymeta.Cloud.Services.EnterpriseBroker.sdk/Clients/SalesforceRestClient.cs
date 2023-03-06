
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Kymeta.Cloud.Services.Toolbox.Rest;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;

public interface ISalesforceRestClient
{
    Task<List<OrderProduct>> GetOrderProducts(string orderNumber, CancellationToken cancellationToken);
    Task<SalesforceResponse<UpdateProductResponse>> SyncFromOracle( OracleSalesforceSyncRequest syncRequest, CancellationToken cancellationToken);
}

public class SalesforceRestClient : ISalesforceRestClient
{
    private readonly HttpClient _client;
    private readonly ILogger<ISalesforceRestClient> _logger;
    private readonly string RequestUri = "/services/data/v56.0/";

    public SalesforceRestClient(HttpClient client, ILogger<ISalesforceRestClient> logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
    }

    public async Task<List<OrderProduct>> GetOrderProducts(string orderKey, CancellationToken cancellationToken)
    {
        var sfOrder = await new RestClient(_client)
            .SetPath($"/query?q={CreateQueryToGetProducts(orderKey)}")
            .SetLogger(_logger)
            .GetAsync(cancellationToken)
            .GetRequiredContent<SalesforceOrder>();
      
        return sfOrder.OrderProducts;
    }

    public async Task<SalesforceResponse<UpdateProductResponse>?> SyncFromOracle(OracleSalesforceSyncRequest syncRequest, CancellationToken cancellationToken)
    {
        return await new RestClient(_client)
        .SetPath($"{RequestUri}composite")
        .SetLogger(_logger)
        .GetAsync(cancellationToken)
        .GetContent<SalesforceResponse<UpdateProductResponse>>();
    }

    private string CreateQueryToGetProducts(string orderKey)
    {
        var filedsToGet = @"Id, OrderId, Product_Code__c, Product2Id, IsDeleted, OriginalOrderItemId, Quantity, SBQQ__BillingFrequency__c,
UnitPrice, NetPrice__c, Ship_Date__c, Oracle_Invoice__c";
        //Number_of_Billing_Periods__c - error
        //ShipToContactId - error
        //Preferred_Contact_Method__c - error
        //Shipping_Terms__c -- error
        //Shipping_Details__c - error
        //BillToAddress__c - error
        // to be tested:
        //Packing_Instructions__c, - error
        //BillToContactId, - error
        //Preferred_Contact_Method__c, - error
        //SBQQ__PaymentTerm__c,- error
        //Sync_Instructions__c , - error
        //Approved__c, - error
        //OrderNumber, - error
        //Oracle_Invoice__c";
        var limitToUnsyncedOrders = " and NEO_Sync_to_Oracle__c=false";
        var query = $"select {filedsToGet} from OrderItem where orderId='{orderKey}' {limitToUnsyncedOrders}";
        return query;
    }


   
}
