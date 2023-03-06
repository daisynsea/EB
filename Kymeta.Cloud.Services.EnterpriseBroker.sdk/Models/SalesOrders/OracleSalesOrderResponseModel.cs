using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;

public record OracleSalesOrderResponseModel
{
    public string? OracleSalesOrderId { get; init; }
    public string? IntegrationStatus { get; init; }
    public string? IntergrationError { get; init; }
}


public class OracleSalesforceSyncRequest
{
    public bool AlOrNone => true;
    public bool CollateSubrequests => true;
    public List<CompositeRequest> CompositeRequest { get; set; } = new List<CompositeRequest>();
}

public class CompositeRequest
{
    public string Method => "PATCH";
    public string Url { get; set; }
    public string ReferenceId { get; set; }
    public OracleOrderStatusSync Body { get; set; }
}

public class OracleOrderStatusSync
{
    public string Oracle_SO__c { get; set; }
    public string Oracle_Status__c { get; set; }
    public string Oracle_Sync_Status__c { get; set; }
    public string NEO_Oracle_Integration_Error__c { get; set; }
    public string NEO_Oracle_Integration_Status__c { get; set; }
    public string NEO_Oracle_Sales_Order_Id__c { get; set; }
    public string NEO_Oracle_Fulfillment_Id__c { get; set; } // this is for product line order
    public string NEO_Oracle_Sales_Order_Line_Id__c { get; set; } // this is for product line order
}

