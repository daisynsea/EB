
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;

public interface ISalesforceRestApi
{
    Task<List<OrderProduct>> GetOrderProducts(string orderNumber, CancellationToken cancellationToken);
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

    public async Task<List<OrderProduct>> GetOrderProducts(string orderKey, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await _client.GetAsync($"/services/data/v56.0/query?q={CreateQueryToGetProducts(orderKey)}", cancellationToken);
        string content = await response.Content.ReadAsStringAsync(cancellationToken);
        var lines = JsonConvert.DeserializeObject<SalesforceOrder>(content);
        return lines.OrderProducts;
    }

    private string CreateQueryToGetProducts(string orderKey)
    {
        var filedsToGet = @"Id, OrderId, Product_Code__c, Product2Id, IsDeleted, OriginalOrderItemId, Quantity, SBQQ__BillingFrequency__c,
UnitPrice, NetPrice__c, Ship_Date__c";
        //Number_of_Billing_Periods__c - error
        //ShipToContactId - error
        //Preferred_Contact_Method__c - error
        //Shipping_Terms__c -- error
        //Shipping_Details__c - error
        //BillToAddress__c - error
        // to be tested:
        //Packing_Instructions__c,
        //BillToContactId,
        //Preferred_Contact_Method__c,
        //SBQQ__PaymentTerm__c,
        //Sync_Instructions__c ,
        //Approved__c,
        //OrderNumber,
        //Oracle_Invoice__c";
        var limitToUnsyncedOrders = " and NEO_Sync_to_Oracle__c=false";
        var query = $"select {filedsToGet} from OrderItem where orderId='{orderKey}' {limitToUnsyncedOrders}";
        return query;
    }
}
