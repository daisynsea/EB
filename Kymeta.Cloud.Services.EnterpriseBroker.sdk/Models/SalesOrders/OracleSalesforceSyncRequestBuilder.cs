namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;

public class OracleSalesforceSyncRequestBuilder
{
    private const string BaseUrl = "/services/data/v57.0/sobjects/";
    private readonly OracleSalesforceSyncRequest _syncRequest;

    private OracleSalesforceSyncRequestBuilder()
    {
        _syncRequest = new OracleSalesforceSyncRequest();
    }

    public static OracleSalesforceSyncRequestBuilder CreateRequest()
    {
        return new OracleSalesforceSyncRequestBuilder();
    }

    public OracleSalesforceSyncRequestBuilder WithSuccessfulOrder(string orderId, string orderNumber, long headerId)
    {
        _syncRequest.CompositeRequest.Add(new CompositeRequest()
        {
            Url = $"{BaseUrl}order/{orderId}",
            ReferenceId = "referenceId",
            Body = new OracleOrderStatusSync()
            {
                Oracle_SO__c = orderNumber,
                Oracle_Status__c = IntegrationConstants.Activated,
                Oracle_Sync_Status__c = IntegrationConstants.Successful,
                NEO_Oracle_Integration_Error__c = IntegrationConstants.Clear,
                NEO_Oracle_Integration_Status__c = IntegrationConstants.Success,
                NEO_Oracle_Sales_Order_Id__c = headerId.ToString()
            }
        });
        return this;
    }
    public OracleSalesforceSyncRequestBuilder WithOrderLine(string orderProductId, string fulfilmentId, string orderLineId)
    {
        int count = _syncRequest.CompositeRequest.Count;
        _syncRequest.CompositeRequest.Add(new CompositeRequest()
        {
            Url = $"{BaseUrl}OrderItem/{orderProductId}",
            ReferenceId = $"refOrderItem{count}",
            Body = new OracleOrderStatusSync()
            {
                NEO_Oracle_Fulfillment_Id__c = fulfilmentId,
                NEO_Oracle_Sales_Order_Line_Id__c = orderLineId
            }
        });
        return this;
    }
    public OracleSalesforceSyncRequest Build()
    {
        return _syncRequest;
    }
}

